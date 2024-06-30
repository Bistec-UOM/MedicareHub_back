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

        /// <summary>
        /// Hashed value string for a password (only for test)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("reg")]
        public ActionResult RegUser(String data)
        {
            var res= _auth.RegisterUser(data);
            return Ok(res);
        }

        /// <summary>
        /// Login : Check if userId matches with password
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Initiate an OTP code to reset password, recieve an Email
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Check if entered OTP code is correct
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Enter new password
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("reset/new")]
        async public Task<ActionResult> NewPassword(NewPassword data)
        {
            await _auth.NewPassword(data);
            return Ok();
        }
    }
}
