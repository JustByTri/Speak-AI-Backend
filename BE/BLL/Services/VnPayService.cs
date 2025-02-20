using Common.DTO.Payment;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Service.IService;
using System.Net.Sockets;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using Common.DTO;
using DAL.Repositories;
using System.Threading.Tasks;
using DAL.IRepositories;
using BLL.Services;
using Microsoft.Extensions.Logging;

namespace Service.Service
{
    public class VnPayService : IVnPayService
    {
        private readonly SortedList<string, string> _requestData = new SortedList<string, string>(new VnPayCompare());
        private readonly IConfiguration _configuration;
        private readonly IVoucherService _voucherService;
        private readonly ILogger<VnPayService> _logger;
        public VnPayService(IConfiguration configuration, IVoucherService voucherService, ILogger<VnPayService> logger)
        {
            _configuration = configuration;
            _voucherService = voucherService;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        }

        public async Task<PaymentRequestDTO> ApplyVoucherAsync(PaymentRequestDTO paymentInfo, string voucherCode)
        {
            if (string.IsNullOrWhiteSpace(voucherCode))
            {
                return paymentInfo; // Không áp dụng voucher nếu không có mã
            }

            _logger.LogInformation($"Checking voucher: {voucherCode}");

            var voucher = await _voucherService.GetVoucherByCode(voucherCode);
            if (voucher == null || !voucher.IsActive)
            {
                _logger.LogError($"Voucher {voucherCode} is invalid or inactive.");
                throw new Exception("Voucher không hợp lệ hoặc đã hết hạn.");
            }

            if (!voucher.IsVoucherValid(paymentInfo.TotalPrice, DateTime.UtcNow))
            {
                throw new Exception("Voucher không hợp lệ hoặc đã hết hạn.");
            }

            decimal discount = voucher.CalculateDiscount(paymentInfo.TotalPrice);
            paymentInfo.TotalPrice -= discount;
            voucher.RemainingQuantity--;

            _logger.LogInformation($"Applied discount: {discount}, New total: {paymentInfo.TotalPrice}, Remaining: {voucher.RemainingQuantity}");
            return paymentInfo;
        }






        public async Task<string> CreatePaymentUrl(PaymentRequestDTO paymentInfo, string voucherCode, HttpContext context)
        {
            // Áp dụng voucher trước khi tạo URL thanh toán
            if (!string.IsNullOrEmpty(voucherCode))
            {
                paymentInfo = await ApplyVoucherAsync(paymentInfo, voucherCode);
            }

            var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById(_configuration["TimeZoneId"]!);
            var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);
            var tick = DateTime.UtcNow.Ticks.ToString();
            var urlCallBack = _configuration["PaymentCallBack:ReturnUrl"];

            AddRequestData("vnp_Version", _configuration["Vnpay:Version"]!);
            AddRequestData("vnp_Command", _configuration["Vnpay:Command"]!);
            AddRequestData("vnp_TmnCode", _configuration["Vnpay:TmnCode"]!);
            AddRequestData("vnp_Amount", ((int)(paymentInfo.TotalPrice * 100)).ToString()); // Giá trị đã được trừ giảm giá
            AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
            AddRequestData("vnp_CurrCode", _configuration["Vnpay:CurrCode"]!);
            AddRequestData("vnp_IpAddr", GetIpAddress(context));
            AddRequestData("vnp_Locale", _configuration["Vnpay:Locale"]!);
            AddRequestData("vnp_OrderInfo", "Upgrade premium");
            AddRequestData("vnp_OrderType", "Upgrade premium");
            AddRequestData("vnp_ReturnUrl", urlCallBack!);
            AddRequestData("vnp_TxnRef", tick);

            return CreateRequestUrl(_configuration["Vnpay:BaseUrl"]!, _configuration["Vnpay:HashSecret"]!);
        }

        private string GetIpAddress(HttpContext context)
        {
            try
            {
                var remoteIpAddress = context.Connection.RemoteIpAddress;
                if (remoteIpAddress != null)
                {
                    if (remoteIpAddress.AddressFamily == AddressFamily.InterNetworkV6)
                    {
                        remoteIpAddress = Dns.GetHostEntry(remoteIpAddress).AddressList
                            .FirstOrDefault(x => x.AddressFamily == AddressFamily.InterNetwork);
                    }

                    return remoteIpAddress?.ToString() ?? "127.0.0.1";
                }
            }
            catch
            {
                return "127.0.0.1";
            }

            return "127.0.0.1";
        }

        private void AddRequestData(string key, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                _requestData[key] = value;
            }
        }

        private string CreateRequestUrl(string baseUrl, string vnpHashSecret)
        {
            var data = new StringBuilder();

            foreach (var (key, value) in _requestData.Where(kv => !string.IsNullOrEmpty(kv.Value)))
            {
                data.Append(WebUtility.UrlEncode(key) + "=" + WebUtility.UrlEncode(value) + "&");
            }

            var querystring = data.ToString().TrimEnd('&');

            var vnpSecureHash = HmacSha512(vnpHashSecret, querystring);
            return $"{baseUrl}?{querystring}&vnp_SecureHash={vnpSecureHash}";
        }

        private string HmacSha512(string key, string inputData)
        {
            using var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(key));
            return BitConverter.ToString(hmac.ComputeHash(Encoding.UTF8.GetBytes(inputData))).Replace("-", "").ToLower();
        }

        public async Task<string> CreatePremiumPaymentUrl(string userId, string premiumPackage, HttpContext context)
        {
            try
            {
                _logger.LogInformation($"Bắt đầu tạo URL thanh toán cho UserId: {userId}, Gói: {premiumPackage}");

                if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(premiumPackage))
                {
                    _logger.LogWarning("UserId hoặc PremiumPackage không hợp lệ.");
                    throw new ArgumentException("Thông tin nâng cấp không hợp lệ.");
                }

                if (!Guid.TryParse(userId, out Guid parsedUserId))
                {
                    _logger.LogWarning($"UserId không hợp lệ: {userId}");
                    throw new ArgumentException("UserId không hợp lệ.");
                }

                // Xác định giá gói Premium
                decimal price = GetPremiumPackagePrice(premiumPackage);
                if (price <= 0)
                {
                    _logger.LogWarning($"Gói Premium không hợp lệ: {premiumPackage}");
                    throw new ArgumentException("Gói Premium không hợp lệ.");
                }

                _logger.LogInformation($"Giá của gói {premiumPackage} là {price} VND.");

                // Tạo request thanh toán
                var paymentRequest = new PaymentRequestDTO
                {
                    UserId = parsedUserId,
                    TotalPrice = price,
                    PaymentMethod = "VNPay",

                };

                // Gọi VNPay để tạo URL thanh toán
                string paymentUrl = await CreatePaymentUrl(paymentRequest, null, context);

                _logger.LogInformation($"URL thanh toán tạo thành công: {paymentUrl}");
                return paymentUrl;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Lỗi khi tạo URL thanh toán: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Lấy giá gói Premium dựa trên tên gói
        /// </summary>
        private decimal GetPremiumPackagePrice(string premiumPackage)
        {
            var priceList = new Dictionary<string, decimal>
    {
        { "silver", 100000 }, // 100k VND
        { "gold", 200000 },   // 200k VND
        { "platinum", 500000 } // 500k VND
    };

            if (string.IsNullOrWhiteSpace(premiumPackage))
            {
                throw new ArgumentException("Tên gói Premium không hợp lệ.");
            }

            premiumPackage = premiumPackage.Trim().ToLower();

            if (!priceList.ContainsKey(premiumPackage))
            {
                throw new ArgumentException($"Gói Premium không hợp lệ. Chỉ chấp nhận: {string.Join(", ", priceList.Keys)}");
            }

            return priceList[premiumPackage];
        }
    }
}
