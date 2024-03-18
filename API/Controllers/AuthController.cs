using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTO;
using Services;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthServices _auth;
        public AuthController(AuthServices authServices) 
        {
            _auth = authServices;
        }

        [HttpPost("reg")]
        public async Task<ActionResult> RegUser(User data)
        {
            var res= await _auth.RegisterUser(data);
            return Ok(res);
        }

        [HttpPost("log")]
        public async Task<ActionResult> LoginUser(UserLog data)
        {
            var res = await _auth.CheckUser(data);
            if (res == "Valid")
            {
                res = await _auth.CreateToken(data.UserId);
                return Ok(res);
            }
            else
            {
                return BadRequest(res);
            }
        }
    }
}
