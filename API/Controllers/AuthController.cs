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

        [HttpPost]
        public async Task<ActionResult<User>> RegUser(User req)
        {
            string paswrdHash=BCrypt.Net.BCrypt.HashPassword(req.Password);
            user = req;
            req.Password = paswrdHash;
            return Ok(user);
        }
    }
}
