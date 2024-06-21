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
        /// <summary>
        /// Retrive All notifications in database
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var notifications = await _context.notification.ToListAsync();
            return Ok(notifications);
        }   
    }

 
}
