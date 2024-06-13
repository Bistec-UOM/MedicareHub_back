using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using Models;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationsController : ControllerBase
    {
        private readonly IHubContext<AdminHub, INotificationClient> _hubContext;
        private readonly ApplicationDbContext _context;

        public NotificationsController(IHubContext<AdminHub, INotificationClient> hubContext, ApplicationDbContext context)
        {
            _hubContext = hubContext;
            _context = context;
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Notification notification)
        {
            // Find the user by UserId
           

            // Send notification using SignalR
            await _hubContext.Clients.All.ReceiveNotification($"{notification.To} -> {notification.Message}");

            // Add the notification to the database context
            notification.To = "Pharmacist";
            notification.From = "System";
            notification.SendAt = DateTime.UtcNow;  // Setting the SendAt time
            _context.notification.Add(notification);

            // Save changes to the database
            await _context.SaveChangesAsync();

            return Ok(notification);
        }
        [HttpPost("get")]
        public async Task<IActionResult> Poster(Notification notification)
        {
            await _hubContext.Clients.All.Receiver($"from receiver {notification.To}->{notification.Message}");
            // Add the notification to the database context
            notification.To = "Doctor";
            notification.From = "System";
            notification.SendAt = DateTime.UtcNow;  // Setting the SendAt time
            _context.notification.Add(notification);

            // Save changes to the database
            await _context.SaveChangesAsync();

            return Ok(notification);

        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var notifications = await _context.notification.ToListAsync();
            return Ok(notifications);
        }   
    }

 
}
