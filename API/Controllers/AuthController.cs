using BCrypt.Net;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public static User user = new User();

        [HttpPost("Reg")]
        public async Task<ActionResult<User>> RegUser(User req)
        {
            string paswrdHash=BCrypt.Net.BCrypt.HashPassword(req.Password);
            user = req;
            user.Password = paswrdHash;
            return Ok(user);
        }

        [HttpPost("Log")]
        public async Task<ActionResult<String>> LogUser(User req)
        {
            if (BCrypt.Net.BCrypt.Verify(req.Password, user.Password))
            {
                return Ok("Valid");
            }
            else
            {
                return BadRequest("Invalid");
            }
        }
    }
}
