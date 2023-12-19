using FSMS.Service.Services.AuthServices;
using FSMS.Service.Utility.Errors;
using FSMS.Service.Utility.Exceptions;
using FSMS.Service.ViewModels.Authentications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;


namespace FSMS.WebAPI.Controllers
{
    [Route("api/auths")]
    [ApiController]
    public class AuthsController : ControllerBase
    {
        private IAuthService _userService;
        private IOptions<JwtAuth> _jwtAuthOptions;

        public AuthsController(IAuthService userService, IOptions<JwtAuth> jwtAuthOptions)
        {
            _userService = userService;
            _jwtAuthOptions = jwtAuthOptions;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] Account account)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = GetModelStateErrors();
                    return BadRequest(new
                    {
                        Message = "Invalid input data",
                        Errors = errors
                    });
                }

                SignInAccount getUserToLogin = _userService.SignInAsync(account, _jwtAuthOptions.Value).Result;

                return Ok(new
                {
                    Data = getUserToLogin
                });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new
                {
                    Message = ex.Message
                });
            }
            catch (BadRequestException ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = ex.Message
                });
            }
        }



        [HttpPost("refresh-token")]
        public IActionResult RefreshToken([FromBody] RefreshTokenRequest refreshTokenRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = GetModelStateErrors();
                    return BadRequest(new
                    {
                        Message = "Invalid input data",
                        Errors = errors
                    });
                }

                RefreshTokenResponse refreshTokenResponse = _userService.RefreshTokenAsync(refreshTokenRequest.RefreshToken, _jwtAuthOptions.Value).Result;

                return Ok(new
                {
                    Data = refreshTokenResponse
                });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new
                {
                    Message = ex.Message
                });
            }
            catch (BadRequestException ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = ex.Message
                });
            }
        }
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest resetPasswordRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = GetModelStateErrors();
                    return BadRequest(new
                    {
                        Message = "Invalid input data",
                        Errors = errors
                    });
                }

                bool isPasswordReset = await _userService.ResetPasswordAsync(resetPasswordRequest.OTP, resetPasswordRequest.Password);

                if (isPasswordReset)
                {
                    return Ok(new
                    {
                        Message = "Password reset successfully"
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        Message = "Invalid OTP or user email"
                    });
                }
            }
            catch (NotFoundException ex)
            {
                return NotFound(new
                {
                    Message = ex.Message
                });
            }
            catch (BadRequestException ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = ex.Message
                });
            }
        }

        [HttpPost("send-otp")]
        public async Task<IActionResult> SendOTP([FromBody] string userEmail)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userEmail))
                {
                    throw new BadRequestException("User email is required.");
                }

                var success = await _userService.SendPasswordResetEmailAndCheckUserAsync(userEmail);

                if (success)
                {
                    return Ok(new
                    {
                        Message = "OTP sent successfully"
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        Message = "User not found"
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = ex.Message
                });
            }
        }
        [HttpPost("logout")]
        public IActionResult Logout([FromBody] string userEmail)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userEmail))
                {
                    throw new BadRequestException("User email is required.");
                }

                SignInAccount signInAccount = new SignInAccount();

                bool isLoggedOut = _userService.LogoutAsync(userEmail, signInAccount).Result;

                if (isLoggedOut)
                {
                    return Ok(new
                    {
                        Message = "Logout successfully"
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        Message = "User not found"
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = ex.Message
                });
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] RegisterAccount registerAccount)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = GetModelStateErrors();
                    return BadRequest(new
                    {
                        Message = "Invalid input data",
                        Errors = errors
                    });
                }

                await _userService.RegisterAccountAsync(registerAccount);

                return Ok(new
                {
                    Message = "Please verify OTP"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = ex.Message
                });
            }
        }


        [HttpPost("confirm-account")]
        public async Task<IActionResult> ConfirmAccount([FromBody] ConfirmAccountRequest confirmAccountRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = GetModelStateErrors();
                    return BadRequest(new
                    {
                        Message = "Invalid input data",
                        Errors = errors
                    });
                }

                bool isAccountConfirmed = await _userService.ConfirmAccountAsync(confirmAccountRequest.OTP);

                if (isAccountConfirmed)
                {
                    return Ok(new
                    {
                        Message = "Account confirmed successfully"
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        Message = "Invalid OTP"
                    });
                }
            }
            catch (NotFoundException ex)
            {
                return NotFound(new
                {
                    Message = ex.Message
                });
            }
            catch (BadRequestException ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = ex.Message
                });
            }
        }



        private List<ErrorDetail> GetModelStateErrors()
        {
            var errors = new List<ErrorDetail>();
            foreach (var pair in ModelState)
            {
                if (pair.Value.Errors.Count > 0)
                {
                    ErrorDetail errorDetail = new ErrorDetail()
                    {
                        FieldNameError = pair.Key,
                        DescriptionError = pair.Value.Errors.Select(error => error.ErrorMessage).ToList()
                    };
                    errors.Add(errorDetail);
                }
            }
            return errors;
        }
    }
}