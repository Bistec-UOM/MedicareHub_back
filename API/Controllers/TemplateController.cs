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

        [HttpGet("tmplt")]
        public async Task<ActionResult<IEnumerable<Test>>> GetAllTests()
        {
            return Ok(await _tmpl.GetAllTests());
        }

        [HttpGet("tmplt/{id}")]
        public async Task<ActionResult<Test>> GetTest(int id)
        {
            var tst = await _tmpl.GetTest(id);
            if (tst == null)
            {
                return NotFound();
            }
            return Ok(tst);
        }

        [HttpPost("tmplt")]
        public async Task<ActionResult<Test>> AddProduct(Test item)
        {
            await _tmpl.Addtest(item);
            return CreatedAtAction(nameof(GetTest), new { id = item.Id }, item);
        }

        [HttpDelete("tmplt/{id}")]
        public async Task<ActionResult> DeleteTest(int id)
        {
            await _tmpl.DeleteTest(id);
            return NoContent();
        }

        [HttpPut("tmpl/{id}")]
        public async Task<ActionResult> EditTest(int id, Test item)
        {
            if (id != item.Id)
            {
                return BadRequest();
            }

            try
            {
                await _tmpl.EditTest(item);
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
