using AppointmentNotificationHandler;
using DataAccessLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Models;
using Services.AdminServices;
using System;
using System.Threading.Tasks;

namespace API.Controllers.AdminControllers
{

    [Authorize(Policy = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly ApplicationDbContext _dbContext;
        private readonly IUserService _userService;
        private readonly IHubContext<AppointmentNotificationHub, IAppointmentNotificationClient> _hubContext;


        public UserController(IUserService userService,ApplicationDbContext dbContext,IHubContext<AppointmentNotificationHub,IAppointmentNotificationClient> hubContext)
        {
            _userService = userService;
            _hubContext = hubContext;
            _dbContext = dbContext;
        }

        /// <summary>
        /// Get all users data from database
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var users = await _userService.GetAllUsers();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Get selected users data from database
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var user = await _userService.GetUser(id);
                if (user == null)
                {
                    return NotFound();
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// add new user to database
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] User value)
        {
            try
            {
                await _userService.AddUser(value);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// update selected user which is in database
        /// </summary>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] User value)
        {
            try
            {
                await _userService.UpdateUser(value);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Delete user which is in database using {id}
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var existingUser = await _userService.GetUser(id);
                if (existingUser == null)
                {
                    return NotFound();
                }
                await _userService.DeleteUser(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
