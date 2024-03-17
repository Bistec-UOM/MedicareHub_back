using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTO;
using Services.AppointmentService;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {

        private readonly AppointmentService _appointment;
        public AppointmentController( AppointmentService appointment)
        {
            
            _appointment = appointment;

        }

        [HttpGet("patient/{id}", Name = "GetPatient")]

        public async Task<ActionResult<Patient>> GetPatient(int id)
        {
            return Ok(await _appointment.GetPatient(id));
        }

        [HttpGet]

        public async Task<ActionResult<ICollection<Appointment>>> GetAllAppointments()
        {
            var appointments = await _appointment.GetAll();

            return Ok(appointments);
        }

        [HttpPost]
        public async Task<ActionResult> AddAppointment(Appointment appointment)
        {
            try
            {
               await _appointment.AddAppointment(appointment);
               return Ok();
            }catch(Exception ex)
            {
               return BadRequest(ex.Message);
            }
        }


        [HttpGet("{id}", Name = "GetaAppointment")]

        public async Task<ActionResult<Appointment>> GetAppointment(int id)
        {
            var appointment = await _appointment.GetAppointment(id);
            return Ok(appointment);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Appointment>> DeleteAppointment(int id)
        {
            var targetAppointment = await _appointment.GetAppointment(id);
            if (targetAppointment is null)
            {
                return NotFound();
            }

            var deletedAppointment = await _appointment.DeleteAppointment(id);
            var targetPatient = await _appointment.GetPatient(deletedAppointment.PatientId);
            if (targetPatient != null)
            {

                var targetEmail = targetPatient.Email;
                var targetday = deletedAppointment.DateTime.Date;
                var targettime = deletedAppointment.DateTime;

                string emailsubject = "appointment update: cancellation notification";
                string username = targetPatient.FullName;
                string emailmessage = "dear " + targetPatient.Name + ",\n" + " we regret to inform you that your scheduled appointment with medicare hub on " + targettime + " has been cancelled. we apologize for any inconvenience this may cause you.";


                EmailSender emailSernder = new EmailSender();
                await emailSernder.SendMail(emailsubject, targetEmail, username, emailmessage);

            }




            return Ok(deletedAppointment);
        }

        [HttpGet("doctor/{doctorId}", Name = "GetDoctorAppointments")]
        public async Task<ActionResult<ICollection<Appointment>>> GetDoctorAppointments(int doctorId)
        {
            var doctorAppointments = await _appointment.GetDoctorAppointments(doctorId);
            return Ok(doctorAppointments);
        }


        [HttpGet("doctor/{doctorId}/day/{date}")]
        public async Task<ActionResult<ICollection<AppointmentWithPatientDetails>>> GetDoctorAppointmentsByDate(int doctorId, DateTime date)
        {
            var doctorDayAppointments = await _appointment.GetDoctorAppointmentsByDate(doctorId, date);
            List<AppointmentWithPatientDetails> appointmentsWithDetails = new List<AppointmentWithPatientDetails>();


            foreach (var appointment in doctorDayAppointments)
            {
                var patientDetails = await _appointment.GetPatient(appointment.PatientId);
                AppointmentWithPatientDetails newappointment = new AppointmentWithPatientDetails
                {
                    Appointment = appointment,
                    patient = patientDetails

                };

                appointmentsWithDetails.Add(newappointment);

            }



            return Ok(appointmentsWithDetails);
        }

        [HttpGet("doctors")]
        public async Task<ActionResult<ICollection<User>>> GetDoctors()
        {
            var doctors = _appointment.GetDoctors();
            return Ok(doctors);
        }

        [HttpPost("patients")]
        public async Task RegisterPatient(Patient patient)
        {

            await _appointment.RegisterPatient(patient);

        }

        [HttpPut("/updateStatus/{id}")]
        public async Task<ActionResult<Appointment>> UpdateAppointmentStatus(int id, [FromBody] Appointment appointment)
        {
            // return Ok(await _repository.UpdateAppointment(id, appointment));
            var targetAppointment = await _appointment.UpdateAppointmentStatus(id, appointment);
            if (targetAppointment is null)
            {
                return NotFound();
            }

            var targetPatient = await _appointment.GetPatient(targetAppointment.PatientId);
            if (targetPatient != null)
            {

                var targetEmail = targetPatient.Email;
                var targetday = targetAppointment.DateTime.Date;
                var targettime = targetAppointment.DateTime;

                string emailsubject = "appointment update: cancellation notification";
                string username = targetPatient.FullName;
                string emailmessage = "dear " + targetPatient.Name + ",\n" + " we regret to inform you that your scheduled appointment with medicare hub on " + targettime + " has been cancelled. we apologize for any inconvenience this may cause you.";


                EmailSender emailSernder = new EmailSender();
                await emailSernder.SendMail(emailsubject, targetEmail, username, emailmessage);

            }




            return Ok(targetAppointment);


        }



        [HttpPut("doctor/{doctorId}/day/{date}")]
        public async Task<ActionResult<List<Appointment>>> CancelAllUpdates(int doctorId, DateTime date)
        {


            var targetCancelledAppointments = _appointment.CancelAllAppointments(doctorId, date);
            foreach (var app in await targetCancelledAppointments)
            {
                var targetPatient = await _appointment.GetPatient(app.PatientId);
                if (targetPatient != null)
                {

                    var targetEmail = targetPatient.Email;
                    var targetday = app.DateTime.Date;
                    var targettime = app.DateTime;

                    string emailsubject = "appointment update: cancellation notification";
                    string username = targetPatient.FullName;
                    string emailmessage = "dear " + targetPatient.Name + ",\n" + " we regret to inform you that your scheduled appointment with medicare hub on " + targettime + " has been cancelled. we apologize for any inconvenience this may cause you.";


                    EmailSender emailSernder = new EmailSender();
                    await emailSernder.SendMail(emailsubject, targetEmail, username, emailmessage);

                }

            }
            return NoContent();
        }

        [HttpGet("doctor/{doctorId}/month/{mId}")]
        public async Task<ActionResult<Appointment>> GetDoctorMonthAppointments(int doctorId, int mId)
        {
            var appointments = await _appointment.GetAppointmentCountOfDays(doctorId, mId);
            return Ok(appointments);
        }

        [HttpGet("patients")]
        public async Task<ActionResult<ICollection<User>>> GetPatients()
        {
            var patients = await _appointment.GetPatients();
            return Ok(patients);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<Appointment>> updateAppointment(int id, [FromBody] Appointment appointment)
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

            var targetDeletedAppointments = _appointment.DeleteAllDoctorDayAppointments(doctorId, date);
            foreach (var app in await targetDeletedAppointments)
            {
                var targetPatient = await _appointment.GetPatient(app.PatientId);
                if (targetPatient != null)
                {

                    var targetEmail = targetPatient.Email;
                    var targetday = app.DateTime.Date;
                    var targettime = app.DateTime;

                    string emailsubject = "appointment update: cancellation notification";
                    string username = targetPatient.FullName;
                    string emailmessage = "dear " + targetPatient.Name + ",\n" + " we regret to inform you that your scheduled appointment with medicare hub on " + targettime + " has been cancelled. we apologize for any inconvenience this may cause you.";


                    EmailSender emailSernder = new EmailSender();
                    await emailSernder.SendMail(emailsubject, targetEmail, username, emailmessage);

                }

            }
            return NoContent();

        }





    }
}
