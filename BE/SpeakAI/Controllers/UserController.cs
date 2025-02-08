using BLL.Interface;
using Common.DTO;
using Common.Message.UserMessage;
using DTO.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Api_InnerShop.Controllers
{
    [Route("api/user-management")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Get address of user by user id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserByUserId(Guid userId)
        {
            var user = await _userService.GetUserResponseDtoByUserId(userId);
            if(user == null)
            {
                return NotFound(new ResponseDTO(UserMessage.UserIdNotExist, 404, false, null));
            }

            return Ok(new ResponseDTO(UserMessage.GetUserSuccessfully, 200, true, user));
        }
    }
}
