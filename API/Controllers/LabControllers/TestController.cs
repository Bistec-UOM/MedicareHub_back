using API.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
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
        private readonly IHubContext<NotificationHub> _hubContext;
        public TestController(TestService test, IHubContext<NotificationHub> hubContext)
        {
            _tst = test;
            _hubContext = hubContext;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMessageToUser(string userId, string message)
        {
            var connectionId = NotificationHub.GetConnectionId(userId);
            if (!string.IsNullOrEmpty(connectionId))
            {
                await _hubContext.Clients.Client(connectionId).SendAsync("ReceiveMessage", message);
                return Ok("Message sent.");
            }

            return NotFound("User not connected.");
        }

        //Get the list of all lab tests to display in test list=================
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
