using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReportFileds>>> GetAllFields()
        {
            return Ok(await _tmpl.GetAllFields());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReportFileds>> GetField(int id)
        {
            var tst = await _tmpl.GetField(id);
            if (tst == null)
            {
                return NotFound();
            }
            return Ok(tst);
        }

        [HttpPost]
        public async Task<ActionResult<ReportFileds>> AddField(ReportFileds item)
        {
            await _tmpl.AddField(item);
            return CreatedAtAction(nameof(GetField), new { id = item.Id }, item);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteFiled(int id)
        {
            await _tmpl.DeleteField(id);
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> EditFiled(int id, ReportFileds item)
        {
            if (id != item.Id)
            {
                return BadRequest();
            }

            try
            {
                await _tmpl.EditField(item);
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
