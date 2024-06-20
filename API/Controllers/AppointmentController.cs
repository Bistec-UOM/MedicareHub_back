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
using System.Diagnostics.Eventing.Reader;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IAppointmentService _appointment;
        private readonly IHubContext<AppointmentNotificationHub, IAppointmentNotificationClient> _hubContext;

        public AppointmentController(ApplicationDbContext dbContext, IAppointmentService appointment, IHubContext<AppointmentNotificationHub, IAppointmentNotificationClient> hubContext)
        {

            _appointment = appointment;
            _dbContext = dbContext;
            _hubContext = hubContext;


        }
        /// <summary>
        /// Get a patient by id
        /// </summary>
        /// <param name="id">patient id</param>
        /// <returns></returns>

        [Authorize(Policy = "Recep")]
        [HttpGet("patient/{id}", Name = "GetPatient")]

        public async Task<ActionResult<Patient>> GetPatient(int id)  //get patient by id
        {
            try
            {
                var patient = await _appointment.GetPatient(id);
                if (patient == null)
                {
                    return NotFound();
                }
                return Ok(await _appointment.GetPatient(id));

            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        /// <summary>
        /// Fetching all appointments
        /// </summary>
        /// <param></param>
        /// <returns></returns>

        [Authorize(Policy = "Doct&Recep")]
        [HttpGet]

        public async Task<IActionResult> GetAllAppointments()   //getting all appointments
        {
            try
            {
                var appointments = await _appointment.GetAll();

                return Ok(appointments);

            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
           
        }
        /// <summary>
        /// Adding a new appointment,Send an email,send real time notification
        /// </summary>
        /// <param name="appointment">new appointment object</param>
        /// <returns></returns>

        [Authorize(Policy = "Recep")]
        [HttpPost]
        public async Task<ActionResult> AddAppointment(Appointment appointment)  //Adding an appointment
        {
            var claim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "RoleId")?.Value;
            if (claim != null)
            {
                int roleId = int.Parse(claim);

                if (roleId == 0)
                {
                    return Unauthorized();
                }

                var app = appointment;
                app.RecepId = roleId;

                try
                {
                    var result = await _appointment.AddAppointment(app);
                    if(result==0)
                    {
                        var doctor = await _dbContext.doctors.FirstOrDefaultAsync(d => d.Id == app.DoctorId); // Get the specific doctor
                        var userId = doctor?.UserId;  //get the user id of the doctor
                        var notification = $"New appointment added for {appointment.DateTime}";





                        Notification newNotification = new Notification();
                        newNotification.Message = notification;
                        newNotification.From = appointment.RecepId.ToString();
                        newNotification.To = appointment.DoctorId.ToString();
                        newNotification.SendAt = DateTime.Now.AddMinutes(330);
                        newNotification.Seen = false;

                        if (userId != null && ConnectionManager._userConnections.TryGetValue(userId.ToString(), out var connectionId))
                        {

                            Debug.WriteLine($"User ConnectionId: {connectionId}");
                            await _hubContext.Clients.Client(connectionId).ReceiveNotification(newNotification);
                            Debug.WriteLine("Notification sent via SignalR.");
                        }



                        await AddNotification(newNotification);


                    }






                    return Ok(result);

                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }else
            {
                return BadRequest();
            }

        }
        /// <summary>
        /// Get an appointment by id
        /// </summary>
        /// <param name="id">appointment id</param>
        /// <returns></returns>

        [Authorize(Policy = "Doct&Recep")]
        [HttpGet("{id}", Name = "GetAppointment")]

        public async Task<ActionResult<Appointment>> GetAppointment(int id)  //getting an appointment by id
        {
            try
            {  
                
                var appointment = await _appointment.GetAppointment(id);
                if (appointment == null)
                {
                    return NotFound();
                }
                return Ok(appointment);

            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
           
        }

        /// <summary>
        /// Deleting an appointment by id
        /// </summary>
        /// <param name="id">Deleted appointment id</param>
        /// <returns></returns>
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

                    var targetEmail = targetPatient.Email ?? "default@gmail.com";
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
        /// <summary>
        /// Getting all the appointments of a specific doctor using docotorId
        /// </summary>
        /// <param name="doctorId">specific doctor id</param>
        /// <returns></returns>

        [Authorize(Policy = "Doct&Recep")]
        [HttpGet("doctor/{doctorId}", Name = "GetDoctorAppointments")]
        public async Task<ActionResult<ICollection<Appointment>>> GetDoctorAppointments(int doctorId)  //getting all the appointments of a specific doctor
        {
            try
            {
                var doctorAppointments = await _appointment.GetDoctorAppointments(doctorId);
                return Ok(doctorAppointments);

            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        /// <summary>
        /// Getting all the appointments of a specific day for a specific doctor
        /// </summary>
        /// <param name="doctorId">The Id of the doctor</param>
        /// <param name="date">The date for which to retrive appointments</param>
        /// <returns></returns>
        [Authorize(Policy = "Doct&Recep")]
        [HttpGet("doctor/{doctorId}/day/{date}")]
        public async Task<ActionResult<ICollection<AppointmentWithPatientDetails>>> GetDoctorAppointmentsByDate(int doctorId, DateTime date)  //getting the appointments with patient details of a specific doc for a specific date
        {

            return Ok(await _appointment.GetDoctorAppointmentsByDateWithPatientDetails(doctorId, date));
        }

        /// <summary>
        /// Getting all the doctors list
        /// </summary>
        /// <returns></returns>
        [Authorize(Policy = "Recep")]
        [HttpGet("doctors")]
        public async Task<ActionResult<ICollection<User>>> GetDoctors() //getting the doctors list
        {
            var doctors = _appointment.GetDoctors();
            return Ok(doctors);
        }
        /// <summary>
        /// Registering a new patient
        /// </summary>
        /// <param name="patient">new patient object</param>
        /// <returns></returns>
        [Authorize(Policy = "Recep")]
        [HttpPost("patients")]
        public async Task<ActionResult> RegisterPatient(Patient patient)  //registering a patient
        {
            try
            {
               await _appointment.RegisterPatient(patient);
               return Ok();

            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

           

        }
        /// <summary>
        /// Cancel the appointment by a doctor,send an email to the patient
        /// </summary>
        /// <param name="id">appoinntment id</param>
         /// <param name="appointment">cancelled appointment object</param>
        /// <returns></returns>
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
                var targetEmail = targetPatient.Email ?? "default@gmail.com";
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
        /// <summary>
        /// Cancel all the appointments by a doctor for a specific day,send  emails to the patients
        /// </summary>
        /// <param name="doctorId">specific doctor id</param>
        /// <param name="date">specific date</param>
        /// <returns></returns>

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
        /// <summary>
        /// Get monthly appointment list of a doctor
        /// </summary>
        /// <param name="doctorId">Specific doctor Id</param>
        /// <param name="mId">MonthId</param>
        /// <returns></returns>
        [Authorize(Policy = "Doct&Recep")]
        [HttpGet("doctor/{doctorId}/month/{mId}")]
        public async Task<ActionResult<Appointment>> GetDoctorMonthAppointments(int doctorId, int mId) //get monthly appointment list for progress bar count
        {
            try
            {
                var appointments = await _appointment.GetAppointmentCountOfDays(doctorId, mId);
                return Ok(appointments);

            }catch (Exception ex)
            {
                return BadRequest(ex.Message);  
            }
            
        }
        /// <summary>
        /// Get patient list
        /// </summary>
        /// <returns></returns>
        [Authorize(Policy = "Recep")]
        [HttpGet("patients")]
        public async Task<ActionResult<ICollection<User>>> GetPatients() //get patients list
        {

            try
            {
                var patients = await _appointment.GetPatients();
                return Ok(patients);

            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }
        /// <summary>
        /// Update the appointment details,send an email
        /// </summary>
        /// <param name="id">Appointment id</param>
        /// <param name="appointment">new updated appointment</param>
        /// <returns></returns>

        [Authorize(Policy = "Recep")]   
        [HttpPut("{id}")]
        public async Task<ActionResult<Appointment>> updateAppointment(int id, [FromBody] Appointment appointment) //update the time of an appointment
        {

            return Ok(await _appointment.UpdateAppointment(id, appointment));
        }
        /// <summary>
        /// Delete all the appointments of a specific doctor of a specific day
        /// </summary>
        /// <param name="doctorId">Specific doctor Id</param>
        /// <param name="date">The day which need to delete appointments</param>
        /// <returns></returns>
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
        /// <summary>
        /// Add a new unable date
        /// </summary>
        /// <param name="uDate">Unable date object</param>
        /// <returns></returns>
        [Authorize(Policy = "Doct")]
        [HttpPost("unableDates")]
        public async Task<ActionResult> AddUnableDate(Unable_Date uDate)  //adding unable dates
        {
            try
            {
                await _appointment.AddUnableDate(uDate);
                return Ok();

            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            

        }

        /// <summary>
        /// Getting unable dates of a specific doctor
        /// </summary>
        /// <param name="doctorId">Specific doctor Id</param>
        /// <returns></returns>
        // [Authorize(Policy = "Doct&Recep")]
        [HttpGet("BlockedDates/{doctorId}")]
        public async Task<ActionResult<ICollection<Unable_Date>>> GetUnableDates(int doctorId)
        {
            try
            {
                var uDates = await _appointment.getUnableDates(doctorId);
                return Ok(uDates);

            }catch (Exception ex) { 
                return BadRequest(ex.Message);
            }
            
        }

        /// <summary>
        /// Get notifications of a specific user
        /// </summary>
        /// <param name="userId">Specific user Id</param>
        /// <returns></returns>
        [HttpGet("Notifications/{userId}")]
        public async Task<ActionResult<ICollection<Notification>>> getNotifications(int userId)
        {
            try
            {
                var notifications = await _appointment.getNotifications(userId);
                return Ok(notifications);

            }catch(Exception ex) { 
            return BadRequest(ex.Message);
             }
           
        }

        /// <summary>
        /// Add a new notification
        /// </summary>
        /// <param name="notification">New notification object</param>
        /// <returns></returns>
        [HttpPost("Notifications")]
        public async Task<ActionResult> AddNotification(Notification notification)  //adding notifications
        {
            try
            {
                await _appointment.AddNotification(notification);
                return Ok();

            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
           

        }
        /// <summary>
        /// Mark an appointment as seen
        /// </summary>
        /// <param name="userId">Specific user Id</param>
        /// <param name="newSeenValue">new bool value of Seen</param>
        /// <returns></returns>
        [HttpPut("notifications/{userId}/user/{newSeenValue}")]
        public async Task MarkAsSeenNotifications(int userId, bool newSeenValue)
        {
            await _appointment.markAsSeenNotifications(userId, newSeenValue);
        }

        /// <summary>
        /// Get Unable time slots of a specific doctor for a specific day
        /// </summary>
        /// <param name="doctorId">Specific doctor Id</param>
        /// <param name="day">specific day</param>
        /// <returns></returns>

        [HttpGet("BlockDates/{doctorId}/date/{day}")]
        public async Task<ActionResult<ICollection<Unable_Date>>> getUnableTimeSlots(int doctorId,DateTime day)
        {
            var results=await _appointment.getUnableTimeslots(doctorId, day);
            return Ok(results);
        }

        /// <summary>
        /// unblock a blocked day
        /// </summary>
        /// <param name="id">unblock day id</param>
        /// <returns></returns>
        [HttpDelete("Unblock/{id}")]
        public async Task<ActionResult<Unable_Date>> UnblockDay(int id)
        {
            var result = await _appointment.UnblockDay(id);
            return Ok(result);


        }











    }
}