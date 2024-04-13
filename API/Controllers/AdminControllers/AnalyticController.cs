using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.AdminServices;
using System.Collections.Generic;

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
        [HttpGet("total-Income")]
        public async Task<IActionResult> GetTotalAmount()
        {
            var res = await _analyticsService.GetTotalAmount();
            return Ok(res);
        }
        [HttpGet("available-count")]
        public async Task<IActionResult> GetAvailableCount()
        {
            var result = await (_analyticsService.GetAvailableCount());
            return Ok(result);
        }
        [HttpGet("daily-drug-usage")]
        public async Task<IActionResult> GetTotalDrugUsage()
        {
            var res = await (_analyticsService.GetTotalDrugUsage());
            return Ok(res);
        }
        [HttpGet("attendance")]
        public async Task<IActionResult> GetAttendance()
        {
            var res = await _analyticsService.GetAttendance();
            return Ok(res);
        }
        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            var res = await _analyticsService.GetUsers();
            return Ok(res);
        }
        [HttpGet("userCheck{date}")]
        public async Task<IActionResult> CheckAttendance(DateTime date)
        {
            var res = await _analyticsService.CheckAttendance(date);
            return Ok(res);
        }
        [HttpGet("lab-report-Count")]
        public async Task<IActionResult> GetLabReports()
        {
            var res = await _analyticsService.GetLabReports();
            return Ok(res);
        }





    }
}
