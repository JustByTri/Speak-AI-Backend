using BLL.Interface;
using Common.Message.AuthMessage;
using Common.Message.EmailMessage;
using Common.Message.GlobalMessage;
using Common.Message.ValidationMessage;
using Common.Template;
using DAL.Entities;
using DTO.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Api_InnerShop.Controllers
{
    [Route("api/email-management")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;
        private readonly IUserService _userService;
        public EmailController(IEmailService emailService, IUserService userService)
        {
            _emailService = emailService;
            _userService = userService;
        }

        /// <summary>
        /// Send OTP for verifying email
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        [HttpPost("send-verify-email")]
        public IActionResult SendVerifiedEmail(string userID)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseDTO(GlobalNotificationMessage.InvalidModel, 400, false));
            }

            var parseUserId = _userService.ParseUserIdToGuid(userID);
            if (parseUserId == Guid.Empty)
            {
                return BadRequest(new ResponseDTO(ValidationErrorMessage.WrongFormatUserId, 400, false));
            }

            var user = _userService.GetUserByUserID(parseUserId);
            if (user == null)
            {
                return BadRequest(new ResponseDTO(ValidationErrorMessage.UserNotFound, 400, false));
            }

            var checkUserVerifiedStatus = _userService.CheckUserVerifiedStatus(parseUserId);
            if (checkUserVerifiedStatus)
            {
                return BadRequest(new ResponseDTO(AuthNotificationMessage.UserIsVerified, 400, false));
            }

            var otpDto = _emailService.GenerateOTP();

            _emailService.SendOTPEmail(user.Email, user.Username, otpDto.OTPCode, EmailSubject.VerifyEmailSubject);
            _userService.SetOtp(otpDto, parseUserId);
            return Ok(new ResponseDTO(EmailNotificationMessage.SendOTPEmailSuccessfully + user.Email, 201, true, otpDto));
        }
    }   
}
