using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTO;
using Services.AppointmentService;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Services.AppointmentService;
using System.Net.Mail;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentRepository _repository;
        public AppointmentController(IAppointmentRepository repository)
        {
            _repository = repository;

        }

        [HttpGet]

        public async Task<ActionResult<ICollection<Appointment>>> GetAllAppointments()
        {
            var appointments = await _repository.GetAll();

            return Ok(appointments);
        }

        [HttpPost]
        public async Task AddAppointment(Appointment appointment)
        {
            await _repository.AddAppointment(appointment);
        }
        [HttpGet("{id}", Name = "GetAppointment")]

        public async Task<ActionResult<Appointment>> GetAppointment(int id)
        {
            var appointment = await _repository.GetAppointment(id);
            return Ok(appointment);
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult<Appointment>> DeleteAppointment(int id)
        {
            var targetAppointment = await _repository.GetAppointment(id);
            if (targetAppointment is null)
            {
                return NotFound();
            }

           var deletedAppointment= _repository.DeleteAppointment(id);
           var targetPatient=await _repository.GetPatient(deletedAppointment.Result.PatientId);
            if(targetPatient!=null)
            {

                var targetEmail=targetPatient.Email;
                var targetday = deletedAppointment.Result.DateTime.Date;
                var targettime = deletedAppointment.Result.DateTime;

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
            var doctorAppointments = await _repository.GetDoctorAppointments(doctorId);
            return Ok(doctorAppointments);
        }
        [HttpGet("doctor/{doctorId}/day/{date}")]
        public async Task<ActionResult<ICollection<AppointmentWithPatientDetails>>> GetDoctorAppointmentsByDate(int doctorId, DateTime date)
        {
            var doctorDayAppointments = await _repository.GetDoctorAppointmentsByDate(doctorId, date);
            List<AppointmentWithPatientDetails> appointmentsWithDetails = new List<AppointmentWithPatientDetails>();


            foreach (var appointment in doctorDayAppointments)
            {
                var patientDetails = await _repository.GetPatient(appointment.PatientId);
                AppointmentWithPatientDetails newappointment = new AppointmentWithPatientDetails
                {
                    Appointment = appointment,
                    patient = patientDetails

                };

                appointmentsWithDetails.Add(newappointment);

            }



            return Ok(appointmentsWithDetails);
        }


        [HttpDelete("doctor/{doctorId}/day/{date}")]
        public async Task<ActionResult> DeleteDoctorAllDayAppointments(int doctorId, DateTime date)
        {
            var targetAppointments = await _repository.GetDoctorAppointmentsByDate(doctorId, date);
            if (targetAppointments is null)
            {
                return NotFound();
            }

          var targetDeletedAppointments=  _repository.DeleteAllDoctorDayAppointments(doctorId, date);
          foreach(var app in await targetDeletedAppointments)
          {
                var targetPatient = await _repository.GetPatient(app.PatientId);
                if (targetPatient != null)
                {

                    var targetEmail = targetPatient.Email;
                    var targetday = app.DateTime.Date;
                    var targettime =app.DateTime;

                    string emailsubject = "appointment update: cancellation notification";
                    string username = targetPatient.FullName;
                    string emailmessage = "dear " + targetPatient.Name + ",\n" + " we regret to inform you that your scheduled appointment with medicare hub on " + targettime + " has been cancelled. we apologize for any inconvenience this may cause you.";


                    EmailSender emailSernder = new EmailSender();
                    await emailSernder.SendMail(emailsubject, targetEmail, username, emailmessage);

                }

            }
          return NoContent();

        }

        [HttpGet("doctors")]
        public async Task<ActionResult<ICollection<User>>> GetDoctors()
        {
            var doctors = _repository.GetDoctors();
            return Ok(doctors);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Appointment>> updateAppointment(int id, [FromBody] Appointment appointment)
        {

            return Ok(await _repository.UpdateAppointment(id, appointment));
        }

        [HttpGet("patients")]
        public async Task<ActionResult<ICollection<User>>> GetPatients()
        {
            var patients = await _repository.GetPatients();
            return Ok(patients);
        }

        [HttpPost("patients")]
        public async Task RegisterPatient(Patient patient)
        {

            await _repository.RegisterPatient(patient);

        }

        [HttpPut("/updateStatus/{id}")]
        public async Task<ActionResult<Appointment>> UpdateAppointmentStatus(int id, [FromBody] Appointment appointment)
        {
           // return Ok(await _repository.UpdateAppointment(id, appointment));
           var targetAppointment=await _repository.UpdateAppointmentStatus(id, appointment);
            if (targetAppointment is null)
            {
                return NotFound();
            }

            var targetPatient = await _repository.GetPatient(targetAppointment.PatientId);
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
        public async Task<ActionResult<List<Appointment>>> CancelAllUpdates(int doctorId,DateTime date)
        {


            var targetCancelledAppointments = _repository.CancelAllAppointments(doctorId, date);
            foreach (var app in await targetCancelledAppointments)
            {
                var targetPatient = await _repository.GetPatient(app.PatientId);
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

        [HttpGet("patient/{id}", Name="GetPatient")]

        public async Task<ActionResult<Patient>> GetPatient(int id)
        {
            return Ok(await _repository.GetPatient(id));
        }

        [HttpGet("doctor/{doctorId}/month/{mId}")]
        public async Task<ActionResult<Appointment>> GetDoctorMonthAppointments(int doctorId,int mId)
        {
            var appointments=await _repository.GetAppointmentCountOfDays(doctorId, mId);
            return Ok(appointments);
        }




    }
}
