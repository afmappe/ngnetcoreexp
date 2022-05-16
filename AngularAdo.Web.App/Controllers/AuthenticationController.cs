using AngularAdo.Web.App.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Linq;

namespace AngularAdo.Web.App.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]", Name = "AuthenticationV1")]
    public class AuthenticationController
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly string _tokenAudience;
        private readonly string _tokenAuthority;
        private readonly string _tokenKey;

        public AuthenticationController(IConfiguration configuration, IHttpContextAccessor contextAccessor)
        {
            _configuration = configuration;
            _tokenAudience = _configuration.GetValue<string>(SettingKeys.Authentication_Audience);
            _tokenAuthority = _configuration.GetValue<string>(SettingKeys.Authentication_Authority);
            _tokenKey = _configuration.GetValue<string>(SettingKeys.Authentication_Tokenkey);
            _contextAccessor = contextAccessor;
        }

        [AllowAnonymous]
        [HttpPost("LoginUser")]
        public ActionResult<string> LoginUser(LoginUserRequest request)
        {
            var token = GenerateUserToken(Guid.NewGuid().ToString("n"), request.UserName);
            return new OkObjectResult(token);
        }

        [HttpGet("message")]
        public ActionResult<string> TestUser(string message)
        {
            string result = null;
            var user = _contextAccessor.HttpContext.User;

            if (user != null)
            {
                var claim = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name);
                result = $"{message} - {claim}";
            }
            return new OkObjectResult(result);
        }

        private string GenerateUserToken(string userId, string userName)
        {
            string result = null;

            if (!string.IsNullOrEmpty(userId))
            {
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                byte[] key = Encoding.ASCII.GetBytes(_tokenKey);
                SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, userId),
                        new Claim(ClaimTypes.Name, userName),
                    }),
                    Audience = _tokenAudience,
                    Issuer = userName,
                    Expires = DateTime.UtcNow.AddHours(23),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
                result = tokenHandler.WriteToken(token);
            }
            return result;
        }
    }
}