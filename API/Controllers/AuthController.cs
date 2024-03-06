using BCrypt.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public static User user = new User(); // For demo purposes only, not recommended in real applications
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("Reg")]
        public async Task<ActionResult<User>> RegUser(User req)
        {
            string paswrdHash = BCrypt.Net.BCrypt.HashPassword(req.Password);
            user = req;
            user.Password = paswrdHash;
            return Ok(user);
        }

        [HttpPost("Log")]
        public async Task<ActionResult<String>> LogUser(User req)
        {
            if (BCrypt.Net.BCrypt.Verify(req.Password, user.Password))
            {
                string token = createToken(user);
                return Ok(token);
            }
            else
            {
                return BadRequest("Invalid");
            }
        }

        private string createToken(User user)
        {
            List<Claim> claims = new List<Claim> {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim(ClaimTypes.Role, "User"),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value!.PadRight(64, '0')));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
   
}
