using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using SocialNetwork.DTOs.Authorize;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SocialNetwork.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private const int EXPIRES_ACCESSTOKEN_MINUTES = 15;

        private const int EXPIRES_REFRESHTOKEN_MINUTES = 7 * 24 * 60;

        private readonly IAuthorService _authServices;

        private readonly IRefreshTokenService _refreshTokenService;

        private readonly IConfiguration _configuration;

        public LoginController(
            IAuthorService authServices,
            IRefreshTokenService refreshTokenService,
            IConfiguration configuration)
        {
            _authServices = authServices;
            _refreshTokenService = refreshTokenService;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            try
            {
                var token = await _authServices.LoginAsync(loginRequest);

                if (token == null)
                {
                    return NotFound(new BaseResponse
                    {
                        Status = 404,
                        Message = "Not Found User In Server"
                    });
                }

                _authServices.SaveTokenToCookieHttpOnly("access_token", token.AccessToken, EXPIRES_ACCESSTOKEN_MINUTES);

                _authServices.SaveTokenToCookieHttpOnly("refresh_token", token.RefreshToken, EXPIRES_REFRESHTOKEN_MINUTES);

                return Ok(new BaseResponse
                {
                    Status = 200,
                    Message = "Login success"
                });
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("singup")]
        public async Task<ActionResult> SingUp(SingUpRequest singUpRequest)
        {
            try
            {
                var result = await _authServices.SignUpAsync(singUpRequest);
                if (result.Succeeded)
                {
                    return Ok(new BaseResponse
                    {
                        Status = 200,
                        Message = "Sing up success",
                        Data = result.Succeeded
                    });
                }

                return BadRequest(new BaseResponse
                {
                    Status = 400,
                    Message = "Invalid token",
                    Data = result.Errors
                });
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("RefreshToken")]
        [Authorize]
        public async Task<IActionResult> RefreshToken(TokenModel tokenModel)
        {
            try
            {
                bool IsValideToken = await _authServices.ValidateToken(tokenModel);

                if(!IsValideToken)
                {
                    return BadRequest(new BaseResponse
                    {
                        Status = 400,
                        Message = "Invalid token"
                    });
                }

                await _refreshTokenService.UpdateRefreshTokenAsync(tokenModel.RefreshToken);

                var user = await _authServices.GetUserByRefreshToken(tokenModel.RefreshToken);

                var newToken = await _authServices.GenerateJwtToken(user);

                return Ok(new BaseResponse
                {
                    Status = 200,
                    Message = "Refresh token success",
                    Data = newToken
                });

            }
            catch
            {
                return Ok(new BaseResponse
                {
                    Status = 404,
                    Message = "Something went wrong"
                });
            }
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if(string.IsNullOrEmpty(userId))
                {
                    return BadRequest(new BaseResponse
                    {
                        Status = 400,
                        Message = "You must login!"
                    });
                }

                await _authServices.LogoutAsync(userId);

                return Ok(new BaseResponse
                {
                    Status = 200,
                    Message = "Logout success",
                });

            }
            catch
            {
                return Ok(new BaseResponse
                {
                    Status = 404,
                    Message = "Something went wrong"
                });
            }
        }

        [HttpGet("google-login")]
        public IActionResult GoogleLogin()
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = "https://localhost:7072/signin-google"
            };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("signin-google")]
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

            if (!result.Succeeded)
            {
                return BadRequest("Authentication with Google failed!");
            }

            var googleUser = result.Principal;
            var email = googleUser.FindFirstValue(ClaimTypes.Email);
            var name = googleUser.FindFirstValue(ClaimTypes.Name);

            var token = GenerateJwtToken(email, name);

            return Ok(new BaseResponse
            {
                Status = 200,
                Message = "Authentication with Google is successful!",
                Data = token
            });
        }


        private string GenerateJwtToken(string email, string name)
        {
            var claims = new[]
            {
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Name, name),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role, "User")
        };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtConfig:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtConfig:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
