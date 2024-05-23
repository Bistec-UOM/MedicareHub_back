using API.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationsController : ControllerBase
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationsController(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Notification notification)
        {
            // Send notification to a specific user
            await _hubContext.Clients.All.SendAsync("ReceiveNotification", notification.Message);
            return Ok();
        }
    }

    public class Notification
    {
        //public int UserId { get; set; } // Include UserId to specify the target user
        public string Message { get; set; }
    }
}
