using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using SocialNetwork.DTOs.Authorize;
using SocialNetwork.DTOs.Response;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SocialNetwork.Services.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IMapper _mapper;
        private readonly JwtConfig _jwtConfig;
        private readonly UserManager<UserEntity> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthorService(
            IUserRepository userRepository,
            IMapper mapper, IOptionsMonitor<JwtConfig> config, 
            IRefreshTokenService refreshTokenService,
            UserManager<UserEntity> userManager,
            RoleManager<IdentityRole> roleManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _jwtConfig = config.CurrentValue;
            _refreshTokenService = refreshTokenService;
            _userManager = userManager;
            _roleManager = roleManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<TokenModel> LoginAsync(LoginRequest loginRequest)
        {
            var user = await _userRepository.GetLoginAsync(loginRequest);

            if (user == null)
            {
                throw new Exception("Email or password is not correct!");
            }

            var token = await GenerateJwtToken(user);

            return token;
        }
        
        public async Task<TokenModel> GenerateJwtToken(UserEntity user)
        {
            var secretKey = Encoding.UTF8.GetBytes(_jwtConfig.SecretKey);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("ID", Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),

                Issuer = _jwtConfig.Issuer,
                Audience = _jwtConfig.Audience,
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256)
            };

            var userRoles = await _userManager.GetRolesAsync(user);

            foreach(var role in userRoles)
            {
                tokenDescription.Subject.AddClaim(new Claim (ClaimTypes.Role, role.ToString()));
            }

            var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescription);

            var accessToken = tokenHandler.WriteToken(token);
            var refreshToken = GenerateRefreshToken();

            var refreshTokenEntity = new RefreshTokenEntity
            {
                RefreshTokenID = Guid.NewGuid(),
                UserID = user.Id,
                Token = refreshToken,
                ExpiredAt = DateTime.UtcNow.AddDays(30),
                JwtID = token.Id,
                IsUsed = false,
                CreatedAt = DateTime.UtcNow
            };

            await _refreshTokenService.CreateRefreshTokenAsync(refreshTokenEntity);

            return new TokenModel
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };

        }

        private string GenerateRefreshToken()
        {
            var random = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(random);
                return Convert.ToBase64String(random);
            }
        }

        public ClaimsPrincipal ValidateAccessToken(string accessToken)
        {
            try
            {
                var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();

                var screteKeyBytes = Encoding.UTF8.GetBytes(_jwtConfig.SecretKey);

                var tokenValidateParamater = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(screteKeyBytes),
                    ClockSkew = TimeSpan.Zero,
                };

                return tokenHandler.ValidateToken(accessToken, tokenValidateParamater, out var validatedToken);

            }
            catch(Exception e)
            {
                throw new SecurityTokenValidationException("Token is not valid", e);
            }
        }

        public async Task<bool> ValidateToken(TokenModel tokenModel)
        {
            //var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();

            //var screteKeyBytes = Encoding.UTF8.GetBytes(_jwtConfig.SecretKey);

            //var tokenValidateParamater = new TokenValidationParameters
            //{
            //    ValidateIssuer = false,
            //    ValidateAudience = false,
            //    ValidateLifetime = false,
            //    ValidateIssuerSigningKey = true,
            //    IssuerSigningKey = new SymmetricSecurityKey(screteKeyBytes),
            //    ClockSkew = TimeSpan.Zero,
            //};

            var tokenInVerification = ValidateAccessToken(tokenModel.AccessToken);
            //var tokenInVerification = tokenHandler.ValidateToken(tokenModel.AccessToken, tokenValidateParamater, out var validatedToken);

            if (!ValidateHeaderAndPayload(tokenInVerification))
            {
                return false;
            }

            var refreshToken = await _refreshTokenService.GetRefreshTokeByTokenAsync(tokenModel.RefreshToken);

            var jwtID = tokenInVerification.Claims.SingleOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti);

            if (!ValidateTokenInDatabase(refreshToken, jwtID?.Value ?? string.Empty))
            {
                return false;
            }

            return true;
        }

        private bool ValidateHeaderAndPayload(ClaimsPrincipal tokenInVerification)
        {

            if (tokenInVerification?.Identity is System.IdentityModel.Tokens.Jwt.JwtSecurityToken jwtSecurityToken)
            {
                return jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.CurrentCultureIgnoreCase);
            }

            var utcExpire = long.Parse(tokenInVerification?.Claims.SingleOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp)?.Value??string.Empty);

            var expireDate = ConvertUnixTimeStampToDateTime(utcExpire);

            if (expireDate > DateTime.UtcNow)
            {
                return false;
            }

            return true;
        }

        private bool ValidateTokenInDatabase(RefreshTokenEntity refreshToken, string jwtID)
        {

            if (refreshToken == null)
            {
                return false;
            }

            if (refreshToken.IsUsed)
            {
                return false;
            }

            if (jwtID != refreshToken.JwtID)
            {
                return false;
            }

            if (refreshToken.IsRevoked)
            {
                return false;
            }
            return true;
        }

        public async Task<UserEntity> GetUserByRefreshToken(string token)
        {
            var refreshToken = await _refreshTokenService.GetRefreshTokeByTokenAsync(token);

            var user = await _userRepository.GetByIDAsync(refreshToken.UserID);

            return user;
        }

        private DateTime ConvertUnixTimeStampToDateTime(long utcExpire)
        {
            var dateTimeInterval = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            dateTimeInterval.AddSeconds(utcExpire).ToUniversalTime();

            return dateTimeInterval;
        }

        public async Task<IdentityResult> SignUpAsync(SingUpRequest signUpRequest)
        {
            var user = new UserEntity
            {
                UserName = signUpRequest.UserName,
                Email = signUpRequest.Email,
                PhoneNumber = signUpRequest.PhoneNumber
            };

            var result =  await _userManager.CreateAsync(user, signUpRequest.Password);

            if (result.Succeeded)
            {
                if (!await _roleManager.RoleExistsAsync(ApplicationRoleModel.User))
                {
                    await _roleManager.CreateAsync(new IdentityRole(ApplicationRoleModel.User));
                }

                await _userManager.AddToRoleAsync(user, ApplicationRoleModel.User);
            }

            return result;
        }

        public void SaveTokenToCookieHttpOnly(string name, string token, int expiresMinutes)
        { 
            var cookieOption = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = DateTime.UtcNow.AddDays(expiresMinutes),
                SameSite = SameSiteMode.None,
            };
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext == null)
            {
                throw new Exception("HttpContext is not available.");
            }

            httpContext.Response.Cookies.Append(name, token, cookieOption);
        }

        public void RemoveTokenToCookieHttpOnly(string name)
        {
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext == null)
            {
                throw new Exception("HttpContext is not available.");
            }

            httpContext.Response.Cookies.Delete(name);
        }

        public async Task UpdateStatusActiveUser(string userId, bool isActive)
        {
            await _userRepository.UpdateStatusActiveUser(userId, isActive);
        }

        public async Task LogoutAsync(string userId)
        {
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext == null)
            {
                throw new Exception("HttpContext is not available.");
            }

            var refreshToken = httpContext.Request.Cookies["refresh_token"];

            RemoveTokenToCookieHttpOnly("access_token");

            RemoveTokenToCookieHttpOnly("refresh_token");

            if(refreshToken == null)
            {
                throw new Exception("Refresh token is not available.");
            }

            await _refreshTokenService.UpdateRefreshTokenAsync(refreshToken);

            await UpdateStatusActiveUser(userId, false);

        }
    }
}
