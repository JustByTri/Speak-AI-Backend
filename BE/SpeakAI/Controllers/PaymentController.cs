using Common.Constant.Message;
using Common.DTO;
using Common.DTO.Payment;
using Common.Enum;
using DAL.Data;
using DAL.Entities;
using DTO.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service.IService;
using Service.Service;
using System;

namespace Api_InnerShop.Controllers
{
    [Route("api/payments")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IVnPayService _vpnPayService;
        private SpeakAIContext _speakAIContext;

        public PaymentController(IPaymentService paymentService, IVnPayService vpnPayService, SpeakAIContext speakAIContext)
        {
            _paymentService = paymentService;
            _vpnPayService = vpnPayService;
            _speakAIContext = speakAIContext;
        }

        [HttpPost("requests")]
        public async Task<IActionResult> CreatePaymentRequest([FromBody] PaymentRequestDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseDTO(
                    ModelState.ToString()!,
                    StatusCodeEnum.BadRequest
                ));
            }

            var response = await _paymentService.CreatePaymentRequest(request, HttpContext);

            if (!string.IsNullOrEmpty(response))
            {
                return Ok(new ResponseDTO(
                    GeneralMessage.GetSuccess,
                    StatusCodeEnum.Created,
                    true,
                    response
                ));
            }

            return StatusCode(500, new ResponseDTO(
                GeneralMessage.BadRequest,
                StatusCodeEnum.InteralServerError
            ));
        }

        [HttpPost("handle-response")]
        public async Task<IActionResult> HandleResponse([FromBody] PaymentResponseDTO responseInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseDTO(
                    ModelState.ToString()!,
                    StatusCodeEnum.BadRequest
                ));
            }

            var response = await _paymentService.HandlePaymentResponse(responseInfo);

            if (response)
            {
                return Ok(new ResponseDTO(
                    GeneralMessage.GetSuccess,
                    StatusCodeEnum.Created,
                    true
                ));
            }

            return StatusCode(500, new ResponseDTO(
                GeneralMessage.BadRequest,
                StatusCodeEnum.InteralServerError
            ));
        }

        [HttpPost("apply-voucher")]
        public async Task<IActionResult> ApplyVoucher([FromBody] ApplyVoucherRequest request)
        {
            // Khai báo rõ ràng kiểu dữ liệu trả về từ phương thức ApplyVoucher
            bool isSuccess;
            string message;
            decimal discountAmount;

            // Gọi phương thức ApplyVoucher từ service
            (isSuccess, message, discountAmount) = await _paymentService.ApplyVoucher(request);

            if (!isSuccess)
            {
                // Trả về BadRequest với thông báo lỗi
                return BadRequest(new ResponseDTO(
                    message,
                    StatusCodeEnum.BadRequest,
                    false
                ));
            }

            // Trả về Ok với thông báo thành công và số tiền giảm giá
            return Ok(new ResponseDTO(
                message,
                StatusCodeEnum.Created,
                true,
                new { DiscountAmount = discountAmount }
            ));
        }

        [HttpPost("create-payment")]
        public async Task<IActionResult> CreatePayment([FromBody] PaymentRequestDTO request)
        {
            try
            {
                var paymentUrl = await _vpnPayService.CreatePaymentUrl(request, request.VoucherCode, HttpContext);
                return Ok(new { paymentUrl });
            }
            catch (Exception ex)
            {

                return StatusCode(500, new { message = "Lỗi khi tạo thanh toán.", error = ex.Message });
            }
        }

        [HttpGet("vnpay-return")]
        public async Task<IActionResult> VnPayReturn()
        {
            var queryCollection = HttpContext.Request.Query;

            string vnp_ResponseCode = queryCollection["vnp_ResponseCode"];
            string vnp_TxnRef = queryCollection["vnp_TxnRef"];
            string vnp_Amount = queryCollection["vnp_Amount"];

            if (vnp_ResponseCode == "00") // Thanh toán thành công
            {
                var payment = await _paymentService.GetPaymentByTransactionId(vnp_TxnRef);
                if (payment == null)
                {
                    return NotFound(new { message = "Không tìm thấy giao dịch." });
                }

                await _paymentService.SavePaymentHistory(vnp_TxnRef, decimal.Parse(vnp_Amount) / 100, "Success");

                if (payment.PaymentType == "PremiumUpgrade")
                {
                    await _paymentService.UpgradeUserToPremium(payment.UserId, payment.PaymentDescription);
                }

                return Ok(new { message = "Thanh toán thành công!", transactionId = vnp_TxnRef });
            }

            return BadRequest(new { message = "Thanh toán thất bại!" });
        }

        [HttpGet("vnpay-Call-back")]
        public async Task<IActionResult> HandleVnPayResponse()
        {
            var queryCollection = HttpContext.Request.Query;
            var vnp_ResponseCode = queryCollection["vnp_ResponseCode"];
            var orderId = queryCollection["vnp_TxnRef"];
            var amount = decimal.Parse(queryCollection["vnp_Amount"]) / 100; // Chuyển về đơn vị VNĐ

            var paymentHistory = new PaymentHistory
            {
                Id = Guid.NewGuid().ToString(),
                TransactionId = orderId,  // Dùng TransactionId thay vì OrderId
                Amount = amount,
                Status = vnp_ResponseCode == "00" ? "Success" : "Failed",
                CreatedAt = DateTime.UtcNow // Dùng CreatedAt thay vì PaymentDate
            };


            if (vnp_ResponseCode == "00")
            {
                return Ok("Thanh toán thành công!");
            }
            else
            {
                return BadRequest("Thanh toán thất bại!");
            }
        }


        [HttpPost("upgrade-premium")]
        public async Task<IActionResult> UpgradeToPremium([FromBody] UpgradePremiumRequestDTO request)
        {
            if (string.IsNullOrEmpty(request.UserId) || string.IsNullOrEmpty(request.PremiumPackage))
            {
                return BadRequest("Thông tin nâng cấp không hợp lệ.");
            }

            string paymentUrl = await _vpnPayService.CreatePremiumPaymentUrl(request.UserId, request.PremiumPackage, HttpContext);
            return Ok(new { paymentUrl });
        }



    }
}
