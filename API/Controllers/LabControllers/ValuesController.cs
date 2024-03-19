using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.LabService;

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
        public async Task<ActionResult<IEnumerable<object>>> GetPatientPrescriptionData()
        {
            var res = await _vs.RequestList();
            return Ok(res);
        }

        [HttpPost("Accept")]
        async public Task<ActionResult> AccceptSample(int id)
        {
            await _vs.AcceptSample(id);
            return Ok();
        }

        [HttpGet("Accept")]
        async public Task<ActionResult<IEnumerable<LabReport>>> AcceptedSamplesList()
        {
            var tmp=await _vs.AcceptedSamplesList();
            return Ok(tmp);
        }


    }
}
