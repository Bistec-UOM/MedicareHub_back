using AppointmentNotificationHandler;
using DataAccessLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Models;
using Services.AdminServices;
using System;
using System.Threading.Tasks;

namespace API.Controllers.AdminControllers
{
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

        // GET: api/<UserController>
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

        // GET api/<UserController>/5
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

        // POST api/<UserController>
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

        // PUT api/<UserController>/5
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

        // DELETE api/<UserController>/5
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
        [HttpPost("SendCashierMsg")]
        public async Task<IActionResult> SendNotiToCashier()
        {

            var drugAvailability = _dbContext.drugs
            .Where(d => d.Avaliable < 10)
            .Select(d => new
            {
                Name = d.BrandN + "(" + d.Weight + "mg)",
                Available = d.Avaliable
            })
            .ToList();

            var pharmacistConnections = _dbContext.users
                .Where(u => u.Role == "Cashier" && u.ConnectionId != null)
                .Select(u => new
                {
                    connectionId = u.ConnectionId,
                    Id = u.Id
                })
                .ToList();

            var unavailableDrugs = drugAvailability.Select(d => d.Name);
            var message = string.Join(", ", unavailableDrugs) + " drugs are less than 10 available";

            bool messageExists = await _dbContext.notification.AnyAsync(n => n.Message == message);

  //          if (!messageExists)
//            {
                var notifications = new List<Notification>();

                foreach (var connection in pharmacistConnections)
                {
                    var notification = new Notification
                    {
                        From = "System",
                        To = connection.Id.ToString(),
                        Message = message,
                        SendAt = DateTime.Now,
                        Seen = false
                    };
                    await _hubContext.Clients.Client(connection.connectionId).ReceiveNotification(notification);

                    notifications.Add(notification);
                }

                await _dbContext.notification.AddRangeAsync(notifications);
                await _dbContext.SaveChangesAsync();
  //              return Ok();
//            }
            return Ok();
       }

    }
}
