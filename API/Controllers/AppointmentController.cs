using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Models;
using Models.DTO;
using Services.AppointmentService;
using System.Security.Claims;

namespace API.Controllers
{
    [Authorize(Policy = "Recep")]
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {

        private readonly AppointmentService _appointment;
        public AppointmentController(AppointmentService appointment)
        {
            
            _appointment = appointment;

        }
        
        [HttpGet("patient/{id}", Name = "GetPatient")]

        public async Task<ActionResult<Patient>> GetPatient(int id)  //get patient by id
        {
            return Ok(await _appointment.GetPatient(id));
        }

        [HttpGet]

        public async Task<ActionResult<ICollection<Appointment>>> GetAllAppointments()   //getting all appointments
        {
            var appointments = await _appointment.GetAll();

            return Ok(appointments);
        }

        [HttpPost]
        public async Task<ActionResult> AddAppointment(Appointment appointment)  //Adding an appointment
        {
            var claim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "RoleId")?.Value;
            int roleId = int.Parse(claim);

            if (roleId == 0) { return Unauthorized(); }

            var app = appointment;
            app.RecepId=roleId;



            try
                {
                    var result = await _appointment.AddAppointment(app);
                    return Ok(result);

                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }

          
        }


        [HttpGet("{id}", Name = "GetAppointment")]

        public async Task<ActionResult<Appointment>> GetAppointment(int id)  //getting an appointment by id
        {
            var appointment = await _appointment.GetAppointment(id);
            return Ok(appointment);
        }

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
                var targettime = deletedAppointment.DateTime;

                string emailsubject = "appointment update: cancellation notification";
                string username = targetPatient.FullName ?? "Patient";
                string emailmessage = "dear " + targetPatient.Name + ",\n" + " we regret to inform you that your scheduled appointment with medicare hub on " + targettime + " has been cancelled. we apologize for any inconvenience this may cause you.";


                EmailSender emailSernder = new EmailSender();
                await emailSernder.SendMail(emailsubject, targetEmail, username, emailmessage);

            }
            return Ok(deletedAppointment);
        }

        [HttpGet("doctor/{doctorId}", Name = "GetDoctorAppointments")]
        public async Task<ActionResult<ICollection<Appointment>>> GetDoctorAppointments(int doctorId)  //getting all the appointments of a specific doctor
        {
            var doctorAppointments = await _appointment.GetDoctorAppointments(doctorId);
            return Ok(doctorAppointments);
        }


        [HttpGet("doctor/{doctorId}/day/{date}")]
        public async Task<ActionResult<ICollection<AppointmentWithPatientDetails>>> GetDoctorAppointmentsByDate(int doctorId, DateTime date)  //getting the appointments with patient details of a specific doc for a specific date
        {
           
            return Ok(await _appointment.GetDoctorAppointmentsByDateWithPatientDetails(doctorId,date));
        }
        [HttpGet("doctors")]
        public async Task<ActionResult<ICollection<User>>> GetDoctors() //getting the doctors list
        {
            var doctors =  _appointment.GetDoctors();
            return Ok(doctors);
        }
        [HttpPost("patients")]
        public async Task RegisterPatient(Patient patient)  //registering a patient
        {

            await _appointment.RegisterPatient(patient);

        }
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
                var targettime = targetAppointment.DateTime;

                string emailsubject = "appointment update: cancellation notification"; //sending an email after cancelling the appointment by doctor
                string username = targetPatient.FullName ?? "Patient" ;
                string emailmessage = "dear " + targetPatient.Name + ",\n" + " we regret to inform you that your scheduled appointment with medicare hub on " + targettime + " has been cancelled. we apologize for any inconvenience this may cause you.";
                EmailSender emailSernder = new EmailSender();
                await emailSernder.SendMail(emailsubject, targetEmail, username, emailmessage);

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
                    var targettime = app.DateTime;

                    string emailsubject = "appointment update: cancellation notification"; //sending emails after cancelling all appointments by doctor
                    string username = targetPatient.FullName ?? "Patient";
                    string emailmessage = "dear " + targetPatient.Name + ",\n" + " we regret to inform you that your scheduled appointment with medicare hub on " + targettime + " has been cancelled. we apologize for any inconvenience this may cause you.";
                    EmailSender emailSernder = new EmailSender();
                    await emailSernder.SendMail(emailsubject, targetEmail, username, emailmessage);

                }
            }
            return NoContent();
        }
        [HttpGet("doctor/{doctorId}/month/{mId}")]
        public async Task<ActionResult<Appointment>> GetDoctorMonthAppointments(int doctorId, int mId) //get monthly appointment list for progress bar count
        {
            var appointments = await _appointment.GetAppointmentCountOfDays(doctorId, mId);
            return Ok(appointments);
        }
        [HttpGet("patients")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
        [HttpPut("{id}")]
        public async Task<ActionResult<Appointment>> updateAppointment(int id, [FromBody] Appointment appointment) //update the time of an appointment
        {

            return Ok(await _appointment.UpdateAppointment(id, appointment));
        }
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
                    var targettime = app.DateTime;

                    string emailsubject = "appointment update: cancellation notification"; //sending emails after deleting prescheduled appointments by a receptionsist
                    string username = targetPatient.FullName ?? "Patient";
                    string emailmessage = "dear " + targetPatient.Name + ",\n" + " we regret to inform you that your scheduled appointment with medicare hub on " + targettime + " has been cancelled. we apologize for any inconvenience this may cause you.";
                    EmailSender emailSernder = new EmailSender();
                    await emailSernder.SendMail(emailsubject, targetEmail, username, emailmessage);
                }

            }
            return NoContent();

        }

        [HttpPost("unableDates")]
        public async Task AddUnableDate(Unable_Date uDate)  //adding unable dates
        {
           await _appointment.AddUnableDate(uDate);
           
        }

        [HttpGet("BlockedDates/{doctorId}")]
        public async Task<ActionResult<ICollection<Unable_Date>>> GetUnableDates(int doctorId)
        {
            var uDates= await _appointment.getUnableDates(doctorId);
            return Ok(uDates);
        }





    }
}
