using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTO;
using Services;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthentController : ControllerBase
    {
        private readonly AuthServices _auth;
        public AuthentController(AuthServices authServices) 
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
            if (res == "OK")
            {
                return Ok(res);
            }
            else
            {
                return BadRequest(res);
            }
        }
    }
}
