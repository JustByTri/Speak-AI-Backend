using BLL.Interface;
using BLL.IService;
using Common.Constant.Message;
using Common.DTO;
using Common.DTO.Query;
using Common.Enum;
using Common.Query;
using DTO.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.IService;
using System.ComponentModel.DataAnnotations;

namespace SpeakAI.Controllers
{
    [Route("api/transactions")]
    [ApiController]
    [Authorize]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly IUserService _userService;

        public TransactionController(ITransactionService transactionService,
                                     IUserService userService)
        {
            _transactionService = transactionService;
            _userService = userService;
        }

        [HttpGet]
        public IActionResult GetAllTransactions([FromQuery] TransactionParameters parameters)
        {
            var response = _transactionService.GetAllTransactions(parameters);

            return Ok(new ResponseDTO(
                GeneralMessage.GetSuccess,
                StatusCodeEnum.OK,
                true,
                response
            ));
        }

        [HttpGet("users/{userId}")]
        public async Task<IActionResult> GetTransOfUser([Required]Guid  userId, [FromQuery] TransactionParameters parameters)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseDTO(
                    ModelState.ToString()!,
                    StatusCodeEnum.BadRequest
                ));
            }

            var user = await _userService.GetUserById(userId);

            if (user != null)
            {
                var response = _transactionService.GetTransOfUser(userId, parameters);

                if (response != null)
                {
                    return Ok(new ResponseDTO(
                        GeneralMessage.GetSuccess,
                        StatusCodeEnum.OK,
                        true,
                        response
                    ));
                }
            }

            return StatusCode(404, new ResponseDTO(
                GeneralMessage.NotFound,
                StatusCodeEnum.NotFound
            ));
        }
    }
}
