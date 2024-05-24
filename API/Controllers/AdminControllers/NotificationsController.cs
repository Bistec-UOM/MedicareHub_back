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
            if (user == null)
            {
                return NotFound("User not found");
            }

            var connectionMessage = notification.connectionId;
            if (connectionMessage!=null)
            {
                await _hubContext.Clients.Client(connectionMessage).ReceiveNotification($"{user.Name}->{notification.Message}");
                return Ok("Notification sent");
            }
            else
            {
                return NotFound("User is not connected");
            }
        }
    }

    public class Notification
    {
        public int UserId { get; set; }
        public string connectionId { get; set; }
        public string Message { get; set; }
    }
}
