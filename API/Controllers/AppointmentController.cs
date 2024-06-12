using AppointmentNotificationHandler;
using DataAccessLayer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Models;
using Models.DTO;
using Services.AppointmentService;
using System.Diagnostics;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace API.Controllers
{
   
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext; 
        private readonly AppointmentService _appointment;
        private readonly IHubContext<AppointmentNotificationHub, IAppointmentNotificationClient> _hubContext;

        public AppointmentController(ApplicationDbContext dbContext,AppointmentService appointment, IHubContext<AppointmentNotificationHub, IAppointmentNotificationClient> hubContext)
        {
            
            _appointment = appointment;
            _dbContext = dbContext;
            _hubContext = hubContext;
           

        }

        [Authorize(Policy = "Recep")]
        [HttpGet("patient/{id}", Name = "GetPatient")]

        public async Task<ActionResult<Patient>> GetPatient(int id)  //get patient by id
        {
            return Ok(await _appointment.GetPatient(id));
        }

          [Authorize(Policy = "Doct&Recep")]
        [HttpGet]

        public async Task<ActionResult<ICollection<Appointment>>> GetAllAppointments()   //getting all appointments
        {
            var appointments = await _appointment.GetAll();

            return Ok(appointments);
        }

        [Authorize(Policy = "Recep")]
        [HttpPost]
        public async Task<ActionResult> AddAppointment(Appointment appointment)  //Adding an appointment
        {
            var claim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "RoleId")?.Value;
            int roleId = int.Parse(claim);

            if (roleId == 0) {
                return Unauthorized();
            }

            var app = appointment;
            app.RecepId=roleId;

            try
                {
                    var result = await _appointment.AddAppointment(app);
                    var doctor = await _dbContext.doctors.FirstOrDefaultAsync(d => d.Id == app.DoctorId); // Get the specific doctor
                    var userId = doctor?.UserId;  //get the user id of the doctor
                    var notification = $"New appointment added for {appointment.DateTime}";

                Notification newNotification = new Notification();
                newNotification.Message = notification;
                newNotification.From = appointment.RecepId.ToString();
                newNotification.To = appointment.DoctorId.ToString();
                newNotification.SendAt = DateTime.Now;
                newNotification.Seen = false;


                if (userId != null && ConnectionManager._userConnections.TryGetValue(userId.ToString(), out var connectionId))
                {
                    Debug.WriteLine($"User ConnectionId: {connectionId}");
                    await _hubContext.Clients.Client(connectionId).ReceiveNotification(newNotification);
                    Debug.WriteLine("Notification sent via SignalR.");
                }
                
                    

                   await AddNotification(newNotification);
              

                return Ok(result);

                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }

          
        }

         [Authorize(Policy = "Doct&Recep")]
        [HttpGet("{id}", Name = "GetAppointment")]

        public async Task<ActionResult<Appointment>> GetAppointment(int id)  //getting an appointment by id
        {
            var appointment = await _appointment.GetAppointment(id);
            return Ok(appointment);
        }

        [Authorize(Policy = "Recep")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Appointment>> DeleteAppointment(int id)  
        {
            var targetAppointment = await _appointment.GetAppointment(id);  //get the sepecific appointment and check whether it exists
            if (targetAppointment is null)
            {
                return NotFound();
            }

            var deletedAppointment = await _appointment.DeleteAppointment(id);  //deleting the appointment
            var targetPatient = await _appointment.GetPatient(deletedAppointment.PatientId);    //sending an email for patient after succesfull appointment cancellation
            if (targetPatient != null)
            {

                var targetEmail = targetPatient.Email??"default@gmail.com" ;
                var targetday = deletedAppointment.DateTime.Date;
                var targettime = deletedAppointment.DateTime.ToString("f");

                string emailSubject = "Appointment Update: Cancellation Notification"; // Sending the cancel notification mail
                string userName = targetPatient.FullName;
                var iconUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRdq0Qw2AUbCppR3IQBWOZx94oZ2NWVuY1vMQ&s";
                string emailMessage = "Dear " + targetPatient.Name + ",<br/><br/> We regret to inform you that your scheduled appointment with Medicare Hub on " + targettime + " has been cancelled. We apologize for any inconvenience this may cause you.";

                var htmlContent = $@"
<html>
<body style='font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 20px;'>
    <div style='max-width: 600px; margin: auto; background-color: #ffffff; padding: 20px; border-radius: 10px; box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);'>
        <h1 style='color: #09D636; text-align: center;'>
            <img src='{iconUrl}' style='margin-right: 8px;' width='32' height='32' alt='Hospital Icon'/>
            Medicare <span style='color: #AFDCBB;'>Hub</span>
        </h1>
        <p style='color: #555555; font-size: 16px;'>{emailMessage}</p>
        <div style='border: 2px solid #FF0000; padding: 15px; border-radius: 10px; background-color: #ffebee;'>
            <h3 style='color: #FF0000; text-align: center;'>Appointment Cancelled</h3>
            <p style='color: #555555; font-size: 16px;'>
                We understand that this may be disappointing and we sincerely apologize for any inconvenience caused. Your well-being is our priority, and we are here to assist you in rescheduling your appointment or addressing any concerns you may have.
            </p>
        </div>
        <p style='font-size: 16px; color: #555;'>
            <span >Our team looks forward to providing you with exceptional care and service.</span> 
            <br/><br/>
            If you have any questions or need further assistance, 
            <span >please do not hesitate to contact us</span>. 
            <br/><br/>
            <span>Thank you for choosing Medicare Hub!</span>
        </p>
        <p style='font-size: 16px; color: #555;'>
            Best regards,
            <br/>
            <span style='color: #007BFF;'>Medicare Hub Team</span>
        </p>
    </div>
</body>
</html>";



                EmailSender emailSernder = new EmailSender();
                await emailSernder.SendMail(emailSubject, targetEmail, userName, htmlContent);

            }
            return Ok(deletedAppointment);
        }

        [Authorize(Policy = "Doct&Recep")]
        [HttpGet("doctor/{doctorId}", Name = "GetDoctorAppointments")]
        public async Task<ActionResult<ICollection<Appointment>>> GetDoctorAppointments(int doctorId)  //getting all the appointments of a specific doctor
        {
            var doctorAppointments = await _appointment.GetDoctorAppointments(doctorId);
            return Ok(doctorAppointments);
        }

        [Authorize(Policy = "Doct&Recep")]
        [HttpGet("doctor/{doctorId}/day/{date}")]
        public async Task<ActionResult<ICollection<AppointmentWithPatientDetails>>> GetDoctorAppointmentsByDate(int doctorId, DateTime date)  //getting the appointments with patient details of a specific doc for a specific date
        {
           
            return Ok(await _appointment.GetDoctorAppointmentsByDateWithPatientDetails(doctorId,date));
        }
        [Authorize(Policy = "Recep")]
        [HttpGet("doctors")]
        public async Task<ActionResult<ICollection<User>>> GetDoctors() //getting the doctors list
        {
            var doctors =  _appointment.GetDoctors();
            return Ok(doctors);
        }
        [Authorize(Policy = "Recep")]
        [HttpPost("patients")]
        public async Task RegisterPatient(Patient patient)  //registering a patient
        {

            await _appointment.RegisterPatient(patient);

        }
        [Authorize(Policy = "Doct&Recep")]
        [HttpPut("/updateStatus/{id}")]
        public async Task<ActionResult<Appointment>> UpdateAppointmentStatus(int id, [FromBody] Appointment appointment)  //cancel the appointment by doctor
        {
            var targetAppointment = await _appointment.UpdateAppointmentStatus(id, appointment);  
            if (targetAppointment is null)  //check updated object exists in the table
            {
                return NotFound();
            }

            var targetPatient = await _appointment.GetPatient(targetAppointment.PatientId);
            if (targetPatient != null)
            {
                var targetEmail = targetPatient.Email ??"default@gmail.com";
                var targetday = targetAppointment.DateTime.Date;
                var targettime = targetAppointment.DateTime.ToString("f");

                string emailSubject = "Appointment Update: Cancellation Notification"; // Sending the cancel notification mail
                string userName = targetPatient.FullName;
                var iconUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRdq0Qw2AUbCppR3IQBWOZx94oZ2NWVuY1vMQ&s";
                string emailMessage = "Dear " + targetPatient.Name + ",<br/><br/> We regret to inform you that your scheduled appointment with Medicare Hub on " + targettime + " has been cancelled. We apologize for any inconvenience this may cause you.";

                var htmlContent = $@"
<html>
<body style='font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 20px;'>
    <div style='max-width: 600px; margin: auto; background-color: #ffffff; padding: 20px; border-radius: 10px; box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);'>
        <h1 style='color: #09D636; text-align: center;'>
            <img src='{iconUrl}' style='margin-right: 8px;' width='32' height='32' alt='Hospital Icon'/>
            Medicare <span style='color: #AFDCBB;'>Hub</span>
        </h1>
        <p style='color: #555555; font-size: 16px;'>{emailMessage}</p>
        <div style='border: 2px solid #FF0000; padding: 15px; border-radius: 10px; background-color: #ffebee;'>
            <h3 style='color: #FF0000; text-align: center;'>Appointment Cancelled</h3>
            <p style='color: #555555; font-size: 16px;'>
                We understand that this may be disappointing and we sincerely apologize for any inconvenience caused. Your well-being is our priority, and we are here to assist you in rescheduling your appointment or addressing any concerns you may have.
            </p>
        </div>
        <p style='font-size: 16px; color: #555;'>
            <span >Our team looks forward to providing you with exceptional care and service.</span> 
            <br/><br/>
            If you have any questions or need further assistance, 
            <span >please do not hesitate to contact us</span>. 
            <br/><br/>
            <span>Thank you for choosing Medicare Hub!</span>
        </p>
        <p style='font-size: 16px; color: #555;'>
            Best regards,
            <br/>
            <span style='color: #007BFF;'>Medicare Hub Team</span>
        </p>
    </div>
</body>
</html>";
                EmailSender emailSernder = new EmailSender();
                await emailSernder.SendMail(emailSubject, targetEmail, userName, htmlContent);

            }
            return Ok(targetAppointment);
        }
       
        [HttpPut("doctor/{doctorId}/day/{date}")]
        public async Task<ActionResult<List<Appointment>>> CancelAllUpdates(int doctorId, DateTime date) //cancel all apps of a day by doctor
        {
            var targetCancelledAppointments = _appointment.CancelAllAppointments(doctorId, date);
            foreach (var app in await targetCancelledAppointments)
            {
                var targetPatient = await _appointment.GetPatient(app.PatientId);
                if (targetPatient != null)
                {

                    var targetEmail = targetPatient.Email ?? "default@gmail.com";
                    var targetday = app.DateTime.Date;
                    var targettime = app.DateTime.ToString("f");
                    string emailSubject = "Appointment Update: Cancellation Notification"; // Sending the cancel notification mail
                    string userName = targetPatient.FullName;
                    var iconUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRdq0Qw2AUbCppR3IQBWOZx94oZ2NWVuY1vMQ&s";
                    string emailMessage = "Dear " + targetPatient.Name + ",<br/><br/> We regret to inform you that your scheduled appointment with Medicare Hub on " + targettime + " has been cancelled. We apologize for any inconvenience this may cause you.";

                    var htmlContent = $@"
<html>
<body style='font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 20px;'>
    <div style='max-width: 600px; margin: auto; background-color: #ffffff; padding: 20px; border-radius: 10px; box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);'>
        <h1 style='color: #09D636; text-align: center;'>
            <img src='{iconUrl}' style='margin-right: 8px;' width='32' height='32' alt='Hospital Icon'/>
            Medicare <span style='color: #AFDCBB;'>Hub</span>
        </h1>
        <p style='color: #555555; font-size: 16px;'>{emailMessage}</p>
        <div style='border: 2px solid #FF0000; padding: 15px; border-radius: 10px; background-color: #ffebee;'>
            <h3 style='color: #FF0000; text-align: center;'>Appointment Cancelled</h3>
            <p style='color: #555555; font-size: 16px;'>
                We understand that this may be disappointing and we sincerely apologize for any inconvenience caused. Your well-being is our priority, and we are here to assist you in rescheduling your appointment or addressing any concerns you may have.
            </p>
        </div>
        <p style='font-size: 16px; color: #555;'>
            <span >Our team looks forward to providing you with exceptional care and service.</span> 
            <br/><br/>
            If you have any questions or need further assistance, 
            <span >please do not hesitate to contact us</span>. 
            <br/><br/>
            <span>Thank you for choosing Medicare Hub!</span>
        </p>
        <p style='font-size: 16px; color: #555;'>
            Best regards,
            <br/>
            <span style='color: #007BFF;'>Medicare Hub Team</span>
        </p>
    </div>
</body>
</html>";
                    EmailSender emailSernder = new EmailSender();
                    await emailSernder.SendMail(emailSubject, targetEmail, userName, htmlContent);

                }
            }
            return NoContent();
        }
        [Authorize(Policy = "Doct&Recep")]
        [HttpGet("doctor/{doctorId}/month/{mId}")]
        public async Task<ActionResult<Appointment>> GetDoctorMonthAppointments(int doctorId, int mId) //get monthly appointment list for progress bar count
        {
            var appointments = await _appointment.GetAppointmentCountOfDays(doctorId, mId);
            return Ok(appointments);
        }
        [Authorize(Policy = "Recep")]
        [HttpGet("patients")]
        public async Task<ActionResult<ICollection<User>>> GetPatients() //get patients list
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var Role = identity.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;

            if(Role=="Receptionist")
            {

                var patients = await _appointment.GetPatients();
                return Ok(patients);

            }
            else
            {
                return Unauthorized();
            }



        }

        [Authorize(Policy = "Recep")]
        [HttpPut("{id}")]
        public async Task<ActionResult<Appointment>> updateAppointment(int id, [FromBody] Appointment appointment) //update the time of an appointment
        {

            return Ok(await _appointment.UpdateAppointment(id, appointment));
        }
        [Authorize(Policy = "Doct&Recep")]
        [HttpDelete("doctor/{doctorId}/day/{date}")]
        public async Task<ActionResult> DeleteDoctorAllDayAppointments(int doctorId, DateTime date)
        {
            var targetAppointments = await _appointment.GetDoctorAppointmentsByDate(doctorId, date);
            if (targetAppointments is null)
            {
                return NotFound();
            }

            var targetDeletedAppointments = _appointment.DeleteAllDoctorDayAppointments(doctorId, date);  //appointment list of a day real deleting by receptionist
            foreach (var app in await targetDeletedAppointments)
            {
                var targetPatient = await _appointment.GetPatient(app.PatientId);
                if (targetPatient != null)
                {
                    var targetEmail = targetPatient.Email ?? "default@gmail.com";
                    var targetday = app.DateTime.Date;
                    var targettime = app.DateTime.ToString("f");

                    //sending emails after deleting prescheduled appointments by a receptionsist
                    string emailSubject = "Appointment Update: Cancellation Notification";
                    string userName = targetPatient.FullName;
                    var iconUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRdq0Qw2AUbCppR3IQBWOZx94oZ2NWVuY1vMQ&s";
                    string emailMessage = "Dear " + targetPatient.Name + ",<br/><br/> We regret to inform you that your scheduled appointment with Medicare Hub on " + targettime + " has been cancelled. We apologize for any inconvenience this may cause you.";

                    var htmlContent = $@"
<html>
<body style='font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 20px;'>
    <div style='max-width: 600px; margin: auto; background-color: #ffffff; padding: 20px; border-radius: 10px; box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);'>
        <h1 style='color: #09D636; text-align: center;'>
            <img src='{iconUrl}' style='margin-right: 8px;' width='32' height='32' alt='Hospital Icon'/>
            Medicare <span style='color: #AFDCBB;'>Hub</span>
        </h1>
        <p style='color: #555555; font-size: 16px;'>{emailMessage}</p>
        <div style='border: 2px solid #FF0000; padding: 15px; border-radius: 10px; background-color: #ffebee;'>
            <h3 style='color: #FF0000; text-align: center;'>Appointment Cancelled</h3>
            <p style='color: #555555; font-size: 16px;'>
                We understand that this may be disappointing and we sincerely apologize for any inconvenience caused. Your well-being is our priority, and we are here to assist you in rescheduling your appointment or addressing any concerns you may have.
            </p>
        </div>
        <p style='font-size: 16px; color: #555;'>
            <span >Our team looks forward to providing you with exceptional care and service.</span> 
            <br/><br/>
            If you have any questions or need further assistance, 
            <span >please do not hesitate to contact us</span>. 
            <br/><br/>
            <span>Thank you for choosing Medicare Hub!</span>
        </p>
        <p style='font-size: 16px; color: #555;'>
            Best regards,
            <br/>
            <span style='color: #007BFF;'>Medicare Hub Team</span>
        </p>
    </div>
</body>
</html>";
                    EmailSender emailSernder = new EmailSender();
                    await emailSernder.SendMail(emailSubject, targetEmail, userName, htmlContent);
                }

            }
            return NoContent();

        }
        [Authorize(Policy = "Doct")]
        [HttpPost("unableDates")]
        public async Task AddUnableDate(Unable_Date uDate)  //adding unable dates
        {
           await _appointment.AddUnableDate(uDate);
           
        }


       // [Authorize(Policy = "Doct&Recep")]
        [HttpGet("BlockedDates/{doctorId}")]
        public async Task<ActionResult<ICollection<Unable_Date>>> GetUnableDates(int doctorId)
        {
            var uDates= await _appointment.getUnableDates(doctorId);
            return Ok(uDates);
        }

        [HttpGet("Notifications/{userId}")]
        public async Task<ActionResult<ICollection<Notification>>> getNotifications(int userId)
        {
            var notifications=await _appointment.getNotifications(userId);
            return Ok(notifications);
        }


        [HttpPost("Notifications")]
        public async Task AddNotification(Notification notification)  //adding notifications
        {
            await _appointment.AddNotification(notification);

        }

        [HttpPut("notifications/{userId}/user/{newSeenValue}")]
        public async Task MarkAsSeenNotifications(int userId,bool newSeenValue)
        {
            await _appointment.markAsSeenNotifications(userId,newSeenValue);
        }











    }
}
