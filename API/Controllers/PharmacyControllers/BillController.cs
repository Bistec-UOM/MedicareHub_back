using AppointmentNotificationHandler;
using DataAccessLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Models;
using Models.DTO;
using Services.PharmacyService;
using System.Diagnostics;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace API.Controllers.PharmacyControllers
{
    [Authorize(Policy = "Cash")]
    [Route("api/[controller]")]
    [ApiController]
    public class BillController : ControllerBase
    {
        private readonly BillService _billService;
        private readonly IHubContext<AppointmentNotificationHub, IAppointmentNotificationClient> _hubContext;
        private readonly ApplicationDbContext _dbContext;

        public BillController(BillService billService, IHubContext<AppointmentNotificationHub, IAppointmentNotificationClient> hubContext, ApplicationDbContext dbcontext)
        {
            _billService = billService;
            _hubContext = hubContext;
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
            //check not available medicines which assigned by a doctor
            var removedData = await _billService.GetMedicinesNotInStock(medicineNames);
            string message = removedData.Count == 0 ? "All medicines are available in our store" : $"{string.Join(", ", removedData)}, are not available in our store which is assigned by doctor";
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

                var notification = new Notification
                {
                    From = "System",
                    To = connection.Id.ToString(),
                    Message = message,
                    SendAt = DateTime.Now.AddMinutes(330),
                    Seen = false
                };


                if (connection.Id!= null && removedData.Count > 0 && ConnectionManager._userConnections.TryGetValue(connection.Id.ToString(), out var connectionId))
                {
                    await _hubContext.Clients.Client(connectionId).ReceiveNotification(notification);
                }
                notifications.Add(notification);
           }
           if(removedData.Count > 0)
            {
                await _dbContext.notification.AddRangeAsync(notifications);
                await _dbContext.SaveChangesAsync();

            }




            //if (medicineDetails == null || medicineDetails.Count == 0)
            //{
            //   return NotFound();
            //}
            return Ok(medicineDetails);
        }

        [Authorize(Policy = "Cash")]
        [HttpPost("AddBillDrugs")]
        public async Task<IActionResult> AddBillDrugs([FromBody] Bill billDrugs)
        {
            var claim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "RoleId")?.Value;
            int roleId = int.Parse(claim);
            try
            {
                await _billService.AddBillDrugs(billDrugs,roleId);
                var noti =  await _billService.ReadNoti();
                if (noti.Message!="")
                {
                    if (noti.To != null && ConnectionManager._userConnections.TryGetValue(noti.To.ToString(), out var connectionId))
                    {
                        await _hubContext.Clients.Client(connectionId).ReceiveNotification(noti);
                    }
                    await _dbContext.notification.AddAsync(noti);
                }



               

                return Ok("Bill drugs added successfully and appointment status updated to 'paid'");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while adding bill drugs: {ex.Message}");
            }
        }

    }
}
