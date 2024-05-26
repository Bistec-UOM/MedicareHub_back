using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using DataAccessLayer;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationsController : ControllerBase
    {
        private readonly IHubContext<NotificationHub, INotificationClient> _hubContext;
        private readonly ApplicationDbContext _context;

        public NotificationsController(IHubContext<NotificationHub, INotificationClient> hubContext, ApplicationDbContext context)
        {
            _hubContext = hubContext;
            _context = context;
        }
        [HttpPost]
        public async Task<IActionResult> Post(Notification notification)
        {
            var user = await _context.users.FindAsync(notification.UserId);      
            await _hubContext.Clients.All.ReceiveNotification($"{user.Name}->{notification.Message}");
            return Ok();
        }
        [HttpPost("get")]
        public async Task<IActionResult> Poster(Notification notification)
        {
            var user = await _context.users.FindAsync(notification.UserId);      
            await _hubContext.Clients.All.Receiver($"from receiver {user.Name}->{notification.Message}");
            return Ok();
        }
    }

    public class Notification
    {
        public int UserId { get; set; }
        public string Message { get; set; }
    }
}
