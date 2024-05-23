using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTO.Lab;
using Models.DTO.Lab.EditTemplate;
using Services.LabService;

namespace API.Controllers.LabControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly TestService _tst;

        public TestController(TestService test)
        {
            _tst = test;
        }

        //Get the list of all lab tests to display in test list=================
        [Authorize(Policy = "Lab")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Test>>> GetAllTests()
        {
            return Ok(await _tst.GetAllTests());
        }

        //Delete a test from available test list=================================
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTest(int id)
        {
            await _tst.DeleteTest(id);
            return Ok();
        }

        //Edit test details from popup box=======================================
        [HttpPut]
        public async Task<ActionResult> EditTest(Test data)
        {
            await _tst.EditTest(data);
            return Ok();
        }

        //Get all fields of a Test====================================
        [HttpGet("Template{id}")]
        public async Task<ActionResult<IEnumerable<ReportFields>>> GetFieldsByTest(int id)
        {
            return Ok(await _tst.GetFieldByTest(id));
        }


        //Add new test (with templates)====================================
        [HttpPost("Template")]
        public async Task<ActionResult> Addtmp(TemplateObj item)
        {
            await _tst.AddTemplate(item);
            return Ok();
        }

        //edit existing template=============================================
        [HttpPut("Template")]
        public async Task<ActionResult> Edittmplt(EdittemplateObj data)
        {
            await _tst.EditTemplate(data);
            return Ok();
        }


    }
}