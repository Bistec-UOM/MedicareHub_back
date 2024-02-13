using Microsoft.AspNetCore.Mvc;
using Models;
using Services;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Prescript_drugController : Controller
    {
        private readonly Prescript_drugService _pre;
        public Prescript_drugController(Prescript_drugService pre)
        {
            _pre = pre;
        }
        [HttpPost]
        public async Task<ActionResult<string>> AddPrescript_drug(Prescript_drug item)
        {
            await _pre.AddPrecript_drug(item);
            return Ok("success");
        }
    }
}
