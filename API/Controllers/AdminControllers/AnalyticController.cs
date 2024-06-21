using AppointmentNotificationHandler;
using DataAccessLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Models;
using Services.AdminServices;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace API.Controllers.AdminControllers
{
    [Authorize(Policy = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AnalyticController : ControllerBase
    {
        private readonly IAnalyticsService _analyticsService;
        private readonly AppointmentNotificationHub _hubContext;
        private readonly ApplicationDbContext _dbContext;

        public AnalyticController(ApplicationDbContext dbContext,IAnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
            _dbContext = dbContext;
        }

        /// <summary>
        /// Retrive number of patients for each date for each gender
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Retrive income on each day
        /// </summary>
        /// <returns></returns>
        [HttpGet("total-Income")]
        public async Task<IActionResult> GetTotalAmount()
        {
            try
            {
                var res = await _analyticsService.GetTotalAmount();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrive Drug available count
        /// </summary>
        /// <returns></returns>
        [HttpGet("available-count")]
        public async Task<IActionResult> GetAvailableCount()
        {
            try
            {
                var result = await _analyticsService.GetAvailableCount();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrive drug usage in each day
        /// </summary>
        /// <returns></returns>
        [HttpGet("daily-drug-usage")]
        public async Task<IActionResult> GetTotalDrugUsage()
        {
            try
            {
                var res = await _analyticsService.GetTotalDrugUsage();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrive attendance of each role in each month
        /// </summary>
        /// <returns></returns>
        [HttpGet("attendance")]
        public async Task<IActionResult> GetAttendance()
        {
            try
            {
                var res = await _analyticsService.GetAttendance();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }


        /// <summary>
        /// Retrive attendance of each role in selected month
        /// </summary>
        /// <returns></returns>
        [HttpGet("userCheck{date}")]
        public async Task<IActionResult> CheckAttendance(DateTime date)
        {
            try
            {
                var res = await _analyticsService.CheckAttendance(date);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrive count of lab reports in each day
        /// </summary>
        /// <returns></returns>
        [HttpGet("lab-report-Count")]
        public async Task<IActionResult> GetLabReports()
        {
            try
            {
                var res = await _analyticsService.GetLabReports();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrive drugs which is less than 10
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetScaredDrugs")]
        public async Task<IActionResult> GetScaredDrugs()
        {
            try
            {
                var res = await _analyticsService.GetScaredDrugs();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }
        
       
    }
}
