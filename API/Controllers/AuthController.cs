using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTO.Auth;
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
        public ActionResult RegUser(String data)
        {
            var res= _auth.RegisterUser(data);
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
                return Unauthorized(res);
            }
        }

        [HttpPost("reset/sendOTP")]
        async public Task<ActionResult> SendOTP(int id)
        {
            string tmp=await _auth.SendOTP(id);
            if (tmp != "")
            {
                return Ok(tmp);
            }
            else
            {
                return BadRequest("User doen't exist");
            }
        }

        [HttpPost("reset/checkOTP")]
        async public Task<ActionResult> CheckOTP(SentOTP data)
        {
            String msg=await _auth.CheckOTP(data);
            if (msg == "OK")
            {
                return Ok();
            }
            else
            {
                return BadRequest(msg);
            }
        }

        [HttpPost("reset/new")]
        async public Task<ActionResult> NewPassword(NewPassword data)
        {
            await _auth.NewPassword(data);
            return Ok();
        }
    }
}
