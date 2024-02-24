using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTO;
using Services;

namespace API.Controllers
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


        [HttpGet("{id}")]//Get a set of fields according to the selected test
        public async Task<ActionResult<IEnumerable<ReportFields>>> GetFieldsByTest(int id)
        {
            return Ok(await _tmpl.GetFieldByTest(id));
        }


        [HttpPost]//Add new test
        public async Task<ActionResult> Addtmp(TemplateObj item)
        {
            await _tmpl.AddTemplate(item);
            return Ok();
        }


        [HttpPut]//edit existing template
        public async Task<ActionResult> Edittmplt(EdittemplateObj data)
        {
            await _tmpl.EditTemplate(data);
            return Ok();
        }

    }
}


