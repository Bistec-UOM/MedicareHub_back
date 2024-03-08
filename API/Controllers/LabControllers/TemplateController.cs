using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTO.Lab;
using Services.LabService;

namespace API.Controllers.LabControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TemplateController : ControllerBase
    {
        private readonly TemplateService _tmpl;

        public TemplateController(TemplateService tmpl)
        {
            _tmpl = tmpl;
        }


        //Get a set of fields according to the selected test===============

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<ReportFields>>> GetFieldsByTest(int id)
        {
            return Ok(await _tmpl.GetFieldByTest(id));
        }



        //Add new test (with templates)====================================

        [HttpPost]
        public async Task<ActionResult> Addtmp(TemplateObj item)
        {
            await _tmpl.AddTemplate(item);
            return Ok();
        }



        //edit existing template=============================================

        [HttpPut]
        public async Task<ActionResult> Edittmplt(EdittemplateObj data)
        {
            await _tmpl.EditTemplate(data);
            return Ok();
        }

    }
}


