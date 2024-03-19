using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.AdminServices;

namespace API.Controllers.AdminControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnalyticController : ControllerBase
    {
        private readonly IAnalyticsService _analyticsService;
        public AnalyticController (IAnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }
        [HttpGet("male-female-patients-count")]
        public async Task<IActionResult> GetMaleFemalePatientsCountAllDays()
        {
            try
            {
                var result = await _analyticsService.GetMaleFemalePatientsCountAllDays();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
