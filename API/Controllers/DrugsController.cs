using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DrugsController : ControllerBase
    {
        private readonly DrugsService _drg;
        public DrugsController(DrugsService drg)
        {
            _drg = drg;
        }
        [HttpPost]
        public async Task<ActionResult<Test>> AddDrug(Drug item)
        {
            await _drg.Addtest(item);
            //return CreatedAtAction(nameof(GetTest), new { id = item.TestId }, item);
        }
    }
}
