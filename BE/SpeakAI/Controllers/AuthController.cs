using BLL.Interface;
using Common.DTO;
using Common.Message.AuthMessage;
using Common.Message.EmailMessage;
using Common.Message.GlobalMessage;
using Common.Message.ValidationMessage;
using Common.Template;
using DAL.Entities;
using DTO.DTO;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace Api_InnerShop.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILoginService _loginService;
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;

        public AuthController(ILoginService loginService, IUserService userService, IEmailService emailService)
        {
            _loginService = loginService;
            _userService = userService;
            _emailService = emailService;
        }
        [HttpPost("sign-in")]
        public IActionResult Login(LoginRequestDTO loginRequestDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseDTO(GlobalNotificationMessage.InvalidModel, 500, false, ModelState));
            }
            var result = _loginService.Login(loginRequestDTO);
            if (result != null)
            {
                return Ok(new ResponseDTO(AuthNotificationMessage.LoginSuccessfully, 201, true, result));
            }
            return BadRequest(new ResponseDTO(AuthNotificationMessage.LoginFailed, 400, false));
        }
        [HttpPost("refresh-token")]
        public IActionResult GetNewTokenFromRefreshToken([FromBody] RequestTokenDTO tokenDTO)
        {
            if (ModelState.IsValid)
            {
                var result = _loginService.RefreshAccessToken(tokenDTO);
                if (result == null || string.IsNullOrEmpty(result.AccessToken))
                {
                    return BadRequest(new ResponseDTO(MessageErrorInRefreshToken.CommonError, 400, false, result));
                }
                return Ok(new ResponseDTO(MessageErrorInRefreshToken.Successfully, 201, true, result));
            }
            return BadRequest(new ResponseDTO(GlobalNotificationMessage.InvalidModel, 500, false));
        }
        [HttpPost("logout")]
        public IActionResult Logout([FromBody] LogOutDTO logoutDTO)
        {
            if (ModelState.IsValid)
            {
                var result = _loginService.Logout(logoutDTO.UserId);
                return Ok(result);
            }
            else
            {
                return Ok(new ResponseDTO(AuthNotificationMessage.LogOutFailed, 400, false));
            }
        }





        /// <summary>
        /// Sign Up For Customer API
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("sign-up-customer")]
        [ProducesResponseType(201, Type = typeof(ResponseDTO))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> SignUpAsCustomer([FromBody] SignUpCustomerDTOResquest model)
        {
            try
            {
                var checkValidation = _userService.CheckValidationSignUpCustomer(model);
                if (!checkValidation.IsSuccess)
                {
                    return BadRequest(checkValidation);
                }

                var addNewCustomer = await _userService.SignUpCustomer(model);

                if (addNewCustomer)
                {
                    var response = new ResponseDTO(AuthNotificationMessage.SignUpSuccessfully, 201, true);
                    return Ok(response);
                }
                else
                {
                    var response = new ResponseDTO(AuthNotificationMessage.SignUpUnsuccessfully, 400, true);
                    return BadRequest(response);
                }

            }
            catch (Exception ex)
            {
                var response = new ResponseDTO(ex.Message, 400, false);
                return BadRequest(response);
            }
        }


    

        /// <summary>
        /// Forgot Password API
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpPost("forgot-password")]
        public IActionResult ForgotPassword(string email)
        {
            var result = _userService.ForgotPassword(email);
            return Ok(result);
        }

        /// <summary>
        /// Reset password API
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("reset-password")]
        public IActionResult ResetPassword(ForgotPasswordModelDTO request)
        {
            var validationResult = _userService.CheckValidationForgotPassword(request);

            if (!validationResult.IsSuccess)
            {
                return BadRequest(validationResult);
            }

            var result = _userService.ResetPassword(request);

            if (result.IsSuccess)
            {
                return StatusCode(201, new ResponseDTO(AuthNotificationMessage.PasswordUpdate, 201, true));
            }
            else
            {
                return BadRequest(new ResponseDTO(AuthNotificationMessage.ResetPassword, 400, false));
            }
        }
        /// <summary>
        /// Verify user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost("verify-user-by-otp")]
        public IActionResult VerifyUser(string userId, string otpCode)
        {
            var parseUserId = _userService.ParseUserIdToGuid(userId);
            if (parseUserId == Guid.Empty)
            {
                return BadRequest(new ResponseDTO(ValidationErrorMessage.WrongFormatUserId, 400, false));
            }

            var checkUserExist = _userService.CheckUserExistByUserId(parseUserId);
            if (!checkUserExist)
            {
                return BadRequest(new ResponseDTO(ValidationErrorMessage.UserNotFound, 400, false));
            }

            var checkUserVerifiedStatus = _userService.CheckUserVerifiedStatus(parseUserId);
            if (checkUserVerifiedStatus)
            {
                return BadRequest(new ResponseDTO(AuthNotificationMessage.UserIsVerified, 400, false));
            }

            var checkOtp = _userService.CheckOTP(parseUserId, otpCode);
            if (!checkOtp)
            {
                return BadRequest(new ResponseDTO(AuthNotificationMessage.OtpInccorect, 400, false));
            }

            var checkOTPExpired = _userService.CheckOTPExpired(parseUserId);
            if (checkOTPExpired)
            {
                return BadRequest(new ResponseDTO(AuthNotificationMessage.OtpExpired, 400, false));
            }

            var result = _userService.VerifyUser(parseUserId);
            if (!result)
            {
                return BadRequest(new ResponseDTO(AuthNotificationMessage.VerifyUnsuccessfully, 500, false));
            }
            return Ok(new ResponseDTO(AuthNotificationMessage.VerifySuccessfully, 201, true));
        }

        /// <summary>
        /// Sign Up For Seller API
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        

     
    
      
    }
}
