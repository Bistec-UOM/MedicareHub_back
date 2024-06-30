using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Models;
using Models.DTO.Lab;
using Models.DTO.Lab.EditTemplate;
using Services.LabService;
using System.Security.Claims;

namespace API.Controllers.LabControllers
{
    [Authorize(Policy = "Lab")]
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly TestService _tst;
        public TestController(TestService test)
        {
            _tst = test;
        }


        /// <summary>
        /// List of all lab tests available
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Test>>> GetAllTests()
        {
            return Ok(await _tst.GetAllTests());
        }


        /// <summary>
        /// Get all fields (whole template) of a lab test 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("Template{id}")]
        public async Task<ActionResult<IEnumerable<ReportFields>>> GetFieldsByTest(int id)
        {
            return Ok(await _tst.GetFieldByTest(id));
        }


        /// <summary>
        /// Add new lab test (with template structure)
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost("Template")]
        public async Task<ActionResult> Addtmp(TemplateObj item)
        {
            try
            {
                await _tst.AddTemplate(item);
                return Ok();
            }
            catch
            {
                return NotFound("This test name already Exist");
            }
        }

        /// <summary>
        /// Edit the basic details of a lab test
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult> EditTest(Test data)
        {
            await _tst.EditTest(data);
            return Ok();
        }

        /// <summary>
        /// Edit template of an existing lab test
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPut("Template")]
        public async Task<ActionResult> Edittmplt(EdittemplateObj data)
        {
            await _tst.EditTemplate(data);
            return Ok();
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTest(int id)
        {
            await _tst.DeleteTest(id);
            return Ok();
        }

    }
}
