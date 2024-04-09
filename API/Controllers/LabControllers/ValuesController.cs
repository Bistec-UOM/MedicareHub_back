using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTO.Lab;
using Services.LabService;
using System.Security.Claims;
using System.Security.Principal;

namespace API.Controllers.LabControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ValueService _vs;
        public ValuesController(ValueService vs) 
        {
            _vs = vs;
        }


        [HttpGet("ReportRequest")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<object>> GetPatientPrescriptionData()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var userRole = identity.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;
            var res = await _vs.RequestList();
            return Ok(res);
        }

        [HttpPost("Accept")]
        async public Task<ActionResult> AccceptSample(int id)
        {
            var tmp= await _vs.AcceptSample(id);
            if (tmp)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("Accept")]
        async public Task<ActionResult<IEnumerable<Object>>> AcceptedSamplesList()
        {
            var tmp=await _vs.AcceptedSamplesList();
            return Ok(tmp);
        }

        [HttpPost("Result")]
        async public Task<ActionResult> UploadResults(Result data)
        {
            var tmp=await _vs.UplaodResults(data);
            if (tmp)
            {
                return Ok(data);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
