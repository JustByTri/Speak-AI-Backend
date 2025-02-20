using BLL.Interface;
using BLL.IService;
using Common.Constant.Payment;
using Common.DTO;
using Common.DTO.Payment;
using DAL.Entities;
using Microsoft.AspNetCore.Http;
using Service.IService;

namespace Service.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly ITransactionService _transactionService;
        private readonly IVnPayService _vnpPayService;
        private readonly IUserService _userService;
        private readonly IVoucherService _voucherService;

        public PaymentService(ITransactionService transactionService,
                              IVnPayService vnpPayService,
                              IUserService userService,
                              IVoucherService voucherService)
        {
            _transactionService = transactionService;
            _vnpPayService = vnpPayService;
            _userService = userService;
            _voucherService = voucherService;
        }



        public async Task<string> CreatePaymentRequest(PaymentRequestDTO paymentInfo, HttpContext context)
        {
            if (!string.IsNullOrEmpty(paymentInfo.VoucherCode)) // Sử dụng VoucherCode thay vì VoucherId
            {
                var voucher = await _voucherService.GetVoucherByCode(paymentInfo.VoucherCode);
                if (voucher != null && voucher.IsVoucherValid(paymentInfo.TotalPrice, DateTime.UtcNow))
                {
                    var discount = voucher.CalculateDiscount(paymentInfo.TotalPrice);
                    paymentInfo.TotalPrice -= discount; // Áp dụng giảm giá từ voucher
                }
                else
                {
                    return "Voucher không hợp lệ hoặc đã hết hạn.";
                }
            }

            var unpaidTrans = _transactionService.GetLastTransOfUser(paymentInfo.UserId);

            if (unpaidTrans != null && unpaidTrans.Status == PaymentConstant.PendingStatus)
            {
                unpaidTrans.Status = PaymentConstant.CancelStatus;
                await _transactionService.UpdateTransaction(unpaidTrans.TransactionId, unpaidTrans);
            }

            var transaction = new Transaction
            {
                PaymentMethod = paymentInfo.PaymentMethod,
                TransactionDate = DateTime.UtcNow,
                Amount = (decimal)paymentInfo.TotalPrice,
                TransactionInfo = PaymentConstant.UnSet,
                TransactionNumber = PaymentConstant.UnSet,
                Status = PaymentConstant.PendingStatus,
                UserId = paymentInfo.UserId,
                VoucherCode = paymentInfo.VoucherCode // Sử dụng VoucherCode thay vì VoucherId
            };

            await _transactionService.AddNewTransaction(transaction);

            return await _vnpPayService.CreatePaymentUrl(paymentInfo, paymentInfo.VoucherCode ?? "", context); // Sử dụng VoucherCode
        }

        public async Task<bool> HandlePaymentResponse(PaymentResponseDTO response)
        {
            var unpaidTrans = _transactionService.GetLastTransOfUser(response.UserId);

            if (unpaidTrans != null && unpaidTrans.Status == PaymentConstant.PendingStatus)
            {
                string notifyDes;

                if (response.IsSuccess)
                {
                    // Cập nhật trạng thái thanh toán thành công
                    unpaidTrans.Status = PaymentConstant.PaidStatus;
                    unpaidTrans.TransactionInfo = response.TransactionInfo;
                    unpaidTrans.TransactionNumber = response.TransactionNumber;

                    // Nếu có voucher, cập nhật trạng thái voucher
                    if (!string.IsNullOrEmpty(unpaidTrans.VoucherId))
                    {
                        var voucher = await _voucherService.GetVoucherById(unpaidTrans.VoucherId);
                        if (voucher != null)
                        {
                            voucher.Status = "Used"; // Cập nhật trạng thái voucher thành "Đã sử dụng"
                            await _voucherService.UpdateVoucherFromDTO(voucher.VoucherId, new VoucherDTO
                            {
                                Status = "Used",
                                // Bạn có thể thêm các thuộc tính khác nếu cần
                            });
                        }
                    }

                    // Nâng cấp người dùng
                    var user = await _userService.GetUserById(response.UserId);
                    if (user != null)
                    {
                        user.IsPremium = true;
                        user.PremiumExpiredTime = DateTime.Now.AddMonths(1); // Đặt thời gian hết hạn Premium
                        await _userService.UpdateUser(user);
                    }

                    notifyDes = "Nâng cấp Premium thành công!";
                }
                else
                {
                    // Cập nhật trạng thái giao dịch thanh toán thất bại
                    unpaidTrans.TransactionInfo = response.TransactionInfo;
                    unpaidTrans.TransactionNumber = response.TransactionNumber;
                    unpaidTrans.Status = PaymentConstant.CancelStatus;

                    notifyDes = "Thanh toán thất bại!";
                }

                await _transactionService.UpdateTransaction(unpaidTrans.TransactionId, unpaidTrans);
                return true;
            }

            return false;
        }

        public async Task<(bool isSuccess, string message, decimal discountAmount)> ApplyVoucher(ApplyVoucherRequest request)
        {
            var voucher = await _voucherService.GetVoucherByCode(request.VoucherCode);

            if (voucher == null)
                return (false, PaymentConst.VoucherInvalid, 0);

            if (!voucher.IsActive || voucher.EndDate < DateTime.UtcNow)
                return (false, PaymentConst.VoucherExpired, 0);

            // Kiểm tra số tiền tối thiểu để sử dụng voucher
            if (voucher.MinPurchaseAmount > request.TotalAmount)
                return (false, PaymentConst.VoucherNotEligible, 0);

            // Tính số tiền giảm giá
            decimal discountAmount = 0;

            if (voucher.DiscountAmount > 0)
            {
                // Nếu voucher có số tiền giảm giá cố định
                discountAmount = voucher.DiscountAmount;
            }
            else if (voucher.DiscountPercentage > 0)
            {
                // Nếu voucher có tỷ lệ giảm giá
                // Nếu voucher có tỷ lệ giảm giá
                discountAmount = request.TotalAmount * (decimal)(voucher.DiscountPercentage / 100);

            }

            // Kiểm tra nếu discountAmount lớn hơn số tiền thanh toán
            if (discountAmount > request.TotalAmount)
            {
                // Trường hợp tính toán được số tiền giảm giá lớn hơn số tiền thanh toán, ta chỉ giảm đến mức số tiền thanh toán
                discountAmount = request.TotalAmount;
            }

            return (true, PaymentConst.VoucherApplied, discountAmount);
        }

        public async Task<PaymentHistory?> GetPaymentByTransactionId(string transactionId)
        {
            if (!Guid.TryParse(transactionId, out Guid transId))
            {
                throw new ArgumentException("Transaction ID không hợp lệ");
            }

            var transaction = await _transactionService.GetTransactionById(transId);
            if (transaction == null) return null;

            return new PaymentHistory
            {
                Id = Guid.NewGuid().ToString(), // Tạo ID mới nếu cần
                TransactionId = transaction.TransactionId.ToString(), // Chuyển Guid thành string nếu cần
                Amount = transaction.Amount,
                Status = transaction.Status,
                PaymentMethod = transaction.PaymentMethod,
                CreatedAt = transaction.TransactionDate, // Xử lý DateTime?
                UserId = transaction.UserId.ToString(), // Chuyển Guid thành string nếu cần
                PaymentDescription = transaction.TransactionInfo, // Thêm PaymentDescription
                PaymentType = transaction.PaymentMethod // Gán PaymentType nếu cần
            };
        }

        public async Task SavePaymentHistory(string transactionId, decimal amount, string status)
        {
            if (!Guid.TryParse(transactionId, out Guid transId))
            {
                throw new ArgumentException("Transaction ID không hợp lệ");
            }

            var transaction = await _transactionService.GetTransactionById(transId);
            if (transaction != null)
            {
                transaction.Status = status;
                transaction.Amount = amount;
                await _transactionService.UpdateTransaction(transId, transaction);
            }
        }

        public async Task UpgradeUserToPremium(string userId, string package)
        {
            var user = await _userService.GetUserById(Guid.Parse(userId));
            if (user != null)
            {
                user.IsPremium = true;
                user.PremiumExpiredTime = DateTime.Now.AddMonths(1); // Hoặc dựa vào package
                await _userService.UpdateUser(user);
            }
        }






    }
}
