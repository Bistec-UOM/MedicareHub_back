using DataAccessLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Models;
using Models.DTO;
using Services;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillController : ControllerBase
    {
        private readonly BillService _billService;
        private readonly IHubContext<NotificationHub, INotificationClient> _notificationHub;
        private readonly ApplicationDbContext _dbContext;

        public BillController(BillService billService, IHubContext<NotificationHub, INotificationClient> notificationHub,ApplicationDbContext dbcontext)
        {
            _billService = billService;
            _notificationHub = notificationHub;
            _dbContext = dbcontext;
        }


        [HttpGet("DrugRequest")]
        public async Task<ActionResult<IEnumerable<object>>> GetPatientPrescriptionData()
        {
            var pe = await _billService.RequestList();
            return Ok(pe);
        }


        [HttpPost("GetMedicineDetails")]
        public async Task<ActionResult<IDictionary<string, List<Drug>>>> GetMedicineDetails([FromBody] List<string> medicineNames)
        {
            var medicineDetails = await _billService.GetMedicineDetails(medicineNames);
            var removedData = await _billService.GetMedicinesNotInStock(medicineNames);
            var message = $"\"{string.Join(", ", removedData)}\", are not available in our store";
            var pharmacistConnections = _dbContext.users
                    .Where(u => u.Role == "Cashier" && u.ConnectionId != null)
                    .Select(u => new
                    {
                        connectionId = u.ConnectionId,
                        Id = u.Id
                    })
                .ToList();


                var notifications = new List<Notification>();

                foreach (var connection in pharmacistConnections)
                {
                    await _notificationHub.Clients.Client(connection.connectionId).ReceiveNotification(message);

                    var notification = new Notification
                    {
                        From = "System",
                        To = connection.Id.ToString(),
                        Message = message,
                        SendAt = DateTime.Now,
                        Seen = false
                    };

                    notifications.Add(notification);
                }
                await _dbContext.notification.AddRangeAsync(notifications);
                await _dbContext.SaveChangesAsync();




            if (medicineDetails == null || medicineDetails.Count == 0)
            {
                return NotFound();
            }
            return Ok(message);
        }

        //Add bill details (paid drugs)
        [HttpPost("AddBillDrugs")]
        public async Task<IActionResult> AddBillDrugs([FromBody] Bill billDrugs)
        {
            try
            {
                await _billService.AddBillDrugs(billDrugs);
                return Ok("Bill drugs added successfully and appointment status updated to 'paid'");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while adding bill drugs: {ex.Message}");
            }
        }

    }
}
