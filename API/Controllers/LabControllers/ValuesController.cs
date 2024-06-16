using AppointmentNotificationHandler;
using DataAccessLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.DTO.Lab.UploadResults;
using Services.AppointmentService;
using Services.LabService;
using System.Diagnostics;
using System.Security.Claims;

namespace API.Controllers.LabControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ValueService _vs;
        private readonly ApplicationDbContext _dbContext;
        private readonly IHubContext<AppointmentNotificationHub, IAppointmentNotificationClient> _hubContext;

        public ValuesController(ValueService vs, IHubContext<AppointmentNotificationHub, IAppointmentNotificationClient> hubContext,ApplicationDbContext dbContext) 
        {
            _vs = vs;
            _hubContext = hubContext;
            _dbContext = dbContext;
        }

        [Authorize(Policy = "Lab")]
        [HttpGet("ReportRequest")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<object>> GetPatientPrescriptionData()
        {
            //var identity = HttpContext.User.Identity as ClaimsIdentity;
            //var userRole = identity.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            var res = await _vs.RequestList();
            return Ok(res);
        }

        [Authorize(Policy = "Lab")]
        [HttpPost("Accept")]
        async public Task<ActionResult> AccceptSample(int id)
        {
            var tmp= await _vs.AcceptSample(id);
            if (tmp)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [Authorize(Policy = "Lab")]
        [HttpGet("Accept")]
        async public Task<ActionResult<IEnumerable<Object>>> AcceptedSamplesList()
        {
            var tmp=await _vs.AcceptedSamplesList();
            return Ok(tmp);
        }

        [Authorize(Policy = "Lab")]
        [HttpPost("Result")]
        async public Task<ActionResult> UploadResults(Result data)
        {
            var claim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "RoleId")?.Value;
            int roleId = int.Parse(claim);

            if (data != null && data.Results!=null)
            {
                var tmp = await _vs.UplaodResults(data,roleId);
                if (tmp)
                {
                    //var dataObj = await _dbContext.labReports
                        //.Include(lr => lr.Prescription)
                        //.ThenInclude(p => p.Appointment)
                        //.ThenInclude(a => a.Patient)
                        //.Include(lr => lr.Prescription)
                        //.ThenInclude(p => p.Appointment)
                        //.ThenInclude(a => a.Doctor)
                        //.ThenInclude(d => d.User)
                        //.Include(lr => lr.Test)
                        //.FirstOrDefaultAsync(lr => lr.Id == data.ReportId);

                    //var labReportInfo = new
                    //{
                    //    PatientName = dataObj.Prescription.Appointment.Patient.Name,
                    //    TestName = dataObj.Test.TestName,
                    //    AcceptedDate = dataObj.AcceptedDate,
                    //    UserId = dataObj.Prescription.Appointment.Doctor.UserId
                    //};
                    //
                    //var sendMail = new EmailSender();
                    //string emsg = "Results of your recent lab test (" + labReportInfo.TestName + ") on " + labReportInfo.AcceptedDate + "" +
                    //    " is ready and available." + labReportInfo.UserId;
                    //string notMsg = "Results of recent lab test (" + labReportInfo.TestName + ") of " + labReportInfo.PatientName + " on " + labReportInfo.AcceptedDate +
                    //" is ready and available.";
                    //
                    //if (data.Servere == true)
                    //{
                    //    emsg = emsg + "It appears that there are some conditions that require immediate attention.Therefore, we strongly recommend that you schedule an appointment with your doctor as soon as possible.";
                    //    notMsg = notMsg + "It appears that there are some conditions that require immediate attention.";
                    //}

                    //Notification newNotification = new Notification();
                    //newNotification.Message = notMsg;
                    //newNotification.From = "1";//Add lab Id when authorized
                    //newNotification.To = labReportInfo.UserId.ToString();
                    //newNotification.SendAt = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, TimeZoneInfo.Local.Id, "Sri Lanka Standard Time");
                    //newNotification.Seen = false;
                    //
                    //await sendMail.SendMail(labReportInfo.TestName + " results", "kwalskinick@gmail.com", labReportInfo.PatientName, emsg);
                    //
                    //if (labReportInfo.UserId != null && ConnectionManager._userConnections.TryGetValue(labReportInfo.UserId.ToString(), out var connectionId))
                    //{
                    //    Debug.WriteLine($"User ConnectionId: {connectionId}");
                    //    await _hubContext.Clients.Client(connectionId).ReceiveNotification(newNotification);
                    //    Debug.WriteLine("Notification sent via SignalR.");
                    //}
                    //
                    //await _dbContext.notification.AddAsync(newNotification);

                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            else
            {
                return BadRequest("Empty");
            }

        }


        //check the results of the report doctor requested is available
        [Authorize(Policy = "Doct")]
        [HttpGet("Result")]
        public async Task<ActionResult> CheckResult(int Pid)
        {
            return Ok(await _vs.CheckResult(Pid));
        }

        [Authorize(Policy = "Doct")]
        [HttpPost("Mark")]//mark a labreport as visited as it is opened
        public async Task<ActionResult> MarkCheck(List<int> ids)
        {
            await _vs.MarkCheck(ids);
            return Ok();
        }
    }
}
