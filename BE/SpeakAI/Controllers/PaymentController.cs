using Common.Constant.Message;
using Common.DTO;
using Common.DTO.Payment;
using Common.Enum;
using DTO.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.IService;
using System;

namespace Api_InnerShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("create-request-payment")]
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

        [HttpPost("handle-response-payment")]
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
    }
}
