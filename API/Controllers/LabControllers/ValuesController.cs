using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTO.Lab.UploadResults;
using Services.LabService;
using System.Security.Claims;

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
            //var identity = HttpContext.User.Identity as ClaimsIdentity;
            //var userRole = identity.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
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
            if (data != null && data.Results!=null)
            {
                var tmp = await _vs.UplaodResults(data);
                if (tmp)
                {
                    return Ok(data);
                }
                else
                {
                    return BadRequest();
                }
            }
            else
            {
                return BadRequest("Empty");
            }

        }


        //check the results of the report doctor requested is available
        [HttpGet("Result")]
        public async Task<ActionResult> CheckResult(int Pid)
        {
            return Ok(await _vs.CheckResult(Pid));
        }

        [HttpPost("Mark")]//mark a labreport as visited as it is opened
        public async Task<ActionResult> MarkCheck(int id)
        {
            Boolean tmp=await _vs.MarkCheck(id);
            if (tmp)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
