using BLL.Interface;
using Common.DTO;
using Common.Enum;
using Common.Message.UserMessage;
using DTO.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Annotations;

namespace Api_InnerShop.Controllers
{
    [Route("api/users")]
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
        [HttpGet("{userId}")]
        [SwaggerOperation(Summary = "Hello")]
        public async Task<IActionResult> GetUserByUserId(Guid userId)
        {
            var user = await _userService.GetUserResponseDtoByUserId(userId);
            if (user == null)
            {
                return NotFound(new ResponseDTO(UserMessage.UserIdNotExist, StatusCodeEnum.NotFound, false, null));
            }

            return Ok(new ResponseDTO(UserMessage.GetUserSuccessfully, StatusCodeEnum.OK, true, user));
        }

        /// <summary>
        /// Update user profile
        /// </summary>
        [HttpPut("{userId}")]
        [SwaggerOperation(Summary = "Update user profile")]
        public async Task<IActionResult> UpdateUserProfile(Guid userId, [FromBody] UpdateUserProfileDTO updateUserProfileDto)
        {
            var result = await _userService.UpdateUserProfileAsync(userId, updateUserProfileDto);
            if (!result)
            {
                return BadRequest(new ResponseDTO("Failed to update user profile", StatusCodeEnum.BadRequest, false, null));
            }

            return Ok(new ResponseDTO("User profile updated successfully", StatusCodeEnum.OK, true, null));
        }


        [HttpPut("ban/{userId}")]

        public async Task<IActionResult> BanUser(Guid userId, [FromQuery] bool isBan)
        {
            var result = await _userService.BanUserAsync(userId, isBan);
            if (!result)
            {
                return NotFound(new ResponseDTO("User not found", StatusCodeEnum.NotFound, false, null));
            }

            string message = isBan ? "User has been banned successfully" : "User has been unbanned successfully";
            return Ok(new ResponseDTO(message, StatusCodeEnum.OK, true, null));
        }

        /// <summary>
        /// Get all users
        /// </summary>
        [HttpGet]
        [SwaggerOperation(Summary = "Get all users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            if (users == null || !users.Any())
            {
                return NotFound(new ResponseDTO("No users found", StatusCodeEnum.NotFound, false, null));
            }

            return Ok(new ResponseDTO("Users retrieved successfully", StatusCodeEnum.OK, true, users));
        }



    }
}
