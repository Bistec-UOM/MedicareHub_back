using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.AppointmentService
{
    public class AppointmentService : IAppointmentRepository
    {

        private readonly ApplicationDbContext _dbcontext;
        public AppointmentService(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;


        }
        
        public async Task AddAppointment(Appointment appointment)
        {
            await _dbcontext.appointments.AddAsync(appointment);
            await _dbcontext.SaveChangesAsync();

            var patientId=appointment.PatientId;
            var patient=await GetPatient(patientId);
            var targetEmail=patient.Email;

            string emailSubject = "Confirmation: Your Appointment with Medicare Hub";
            string userName = patient.FullName;
            string emailMessage = "Dear " + patient.Name + ",\n" + "We're thrilled to confirm your appointment with Medicare Hub scheduled for "+appointment.DateTime;

            EmailSender emailSernder = new EmailSender();
            await emailSernder.SendMail(emailSubject, patient.Email, userName, emailMessage);







        }

        public async Task<List<Appointment>> CancelAllAppointments(int doctorId, DateTime date)
        {
            var appointments = await GetDoctorAppointmentsByDate(doctorId, date);
            List<Appointment> updatedResults = new List<Appointment>();

            foreach (Appointment app in appointments)
            {
                if (app.Status == "new") // Assuming "new" is the status for new appointments
                {
                    var upAppointment = await UpdateOnlyOneAppointmentUsingId(app.Id);
                    updatedResults.Add(upAppointment);
                }
            }

            return updatedResults;
        }

        //comment for locally



        public async Task<List<Appointment>> DeleteAllDoctorDayAppointments(int doctorId, DateTime date)
        {
            var targetAppointments = await GetDoctorAppointmentsByDate(doctorId, date);

            if (targetAppointments != null && targetAppointments.Any())
            {
                // Filter appointments with status "new"
                var appointmentsToDelete = targetAppointments.Where(appointment => appointment.Status == "new").ToList();

                if (appointmentsToDelete.Any())
                {
                    _dbcontext.appointments.RemoveRange(appointmentsToDelete);
                    _dbcontext.SaveChanges();
                   
                }

                return appointmentsToDelete;

            }
            else
            {
                return new List<Appointment>();
            }
        }

        public async Task<Patient> GetPatient(int id)
        {
            var patient = _dbcontext.patients.FirstOrDefaultAsync(p => p.Id == id);
            return await patient;
        }

        public async Task<Appointment> DeleteAppointment(int id)
        {
            var appointmentk=await GetAppointment(id);
            if (appointmentk != null)
            {
                _dbcontext.appointments.Remove(appointmentk);
                _dbcontext.SaveChanges();
                return appointmentk;

            }
            else
            {
                throw new ArgumentException($"Appointment with id {id} not found.");
            }

            //if (appointmentk != null)
            //{
            //    try
            //    {
            //        var targetPatient = await GetPatient(appointmentk.PatientId);

            //var targetemail = targetpatient.email;
            //var targetday = appointmentk.datetime.date;
            //var targettime = appointmentk.datetime.timeofday;


            //string emailsubject = "appointment update: cancellation notification";
            //string username = targetpatient.fullname;
            //string emailmessage = "dear " + targetpatient.name + ",\n" + " we regret to inform you that your scheduled appointment with medicare hub on" + "fdsaf" + "at" + "fdsaf" + "has been cancelled. we apologize for any inconvenience this may cause you.";


            //EmailSender emailsernder = new emailsender();
            //await emailsernder.SendMail(emailsubject, targetemail, username, emailmessage);

            //        _dbcontext.appointments.Remove(appointmentk);

            //        await _dbcontext.SaveChangesAsync();






            //    }
            //    catch (DbUpdateConcurrencyException ex)
            //    {
            //        Console.WriteLine($"Concurrency conflict occurred: {ex.Message}");
            //        throw;
            //    }


            //var pt = appointment.PatientId;
            //var apatient = await GetPatient(pt);

            //if (targetPatient != null)
            //{


            //    _dbcontext.appointments.Remove(appointment);
            //    _dbcontext.SaveChanges();

            //}




            //var targetEmail = targetPatient.Email;
            //var targetDay = appointment.DateTime.Date;
            //var targetTime = appointment.DateTime.TimeOfDay;


            //string emailSubject = "Appointment Update: Cancellation Notification";
            //string userName = targetPatient.FullName;
            //string emailMessage = "Dear " + targetPatient.Name + ",\n" + " We regret to inform you that your scheduled appointment with Medicare Hub on" + targetDay + "at" + targetTime + "has been cancelled. We apologize for any inconvenience this may cause you.";





            //EmailSender emailSernder = new EmailSender();
            //await emailSernder.SendMail(emailSubject, targetPatient.Email, userName, emailMessage);



            //}
            //else
            //{
            //    throw new ArgumentException($"Appointment with id {id} not found.");
            //}

            // var pt = appointment.PatientId;


            //  var targetPatient =await GetPatient(pt);




            //  _dbcontext.appointments.Remove(appointment);
            //            _dbcontext.SaveChanges();





        }

        public async Task<List<Appointment>> GetAll()
        {

            return  _dbcontext.appointments.ToList();

            
        }

        public async Task<Appointment> GetAppointment(int id)
        {
            var appointment =  _dbcontext.appointments.FirstOrDefault(a=>a.Id==id);
            return appointment;
        }

      

        public async Task<List<Appointment>> GetDoctorAppointments(int doctorId)
        {
            var doctorAppointments =  _dbcontext.appointments.Where(a => a.Id == doctorId);
            return doctorAppointments.ToList();
        }

        public async Task<List<Appointment>> GetDoctorAppointmentsByDate(int doctorId, DateTime date)
        {
            var doctorDayAppointments =  _dbcontext.appointments.Where(a => a.DoctorId == doctorId && a.DateTime.Date == date);
            return doctorDayAppointments.ToList();  
            
        }

        public async Task<List<User>> GetDoctors()
        {

            var doctors = _dbcontext.users.Where(d => d.Role == "Doctor");
            return doctors.ToList();
           
        }

       

        public async Task<List<Patient>> GetPatients()
        {
           
            return  _dbcontext.patients.ToList();
        }

        public async Task RegisterPatient(Patient patient)
        {
           await _dbcontext.patients.AddAsync(patient);
            await _dbcontext.SaveChangesAsync();

            string emailSubject = "Your Medicare Hub Membership: A Warm Welcome Awaits!";
            string userName = patient.FullName;
            string emailMessage = "Dear " + patient.Name +"\n"+ "Thank you for choosing Medicare Hub. We're honored to be part of your healthcare journey. Please reach out if you need anything.";

            EmailSender emailSernder= new EmailSender();
           await emailSernder.SendMail(emailSubject,patient.Email,userName,emailMessage);



        }

        public async Task<Appointment> UpdateAppointment(int id, Appointment appointment)


{

            System.Diagnostics.Debug.WriteLine("inside update");
            System.Diagnostics.Debug.WriteLine("newtime", appointment.DateTime.ToString(),appointment.DoctorId);
            var oldAppointment = await GetAppointment(id);

    if (oldAppointment != null)
    {
                // Update properties of the existing appointment

                
            oldAppointment.DateTime = appointment.DateTime;
            oldAppointment.Status = appointment.Status;
            oldAppointment.PatientId = appointment.PatientId;
            oldAppointment.DoctorId = appointment.DoctorId;
            oldAppointment.RecepId = appointment.RecepId;

        try
        {
            await _dbcontext.SaveChangesAsync();
            return oldAppointment;
        }
        catch (DbUpdateConcurrencyException ex)
        {
            // Handle concurrency conflict
            // You can implement custom logic here, such as merging changes or retrying the update
            Console.WriteLine($"Concurrency conflict occurred: {ex.Message}");
            throw;
        }
    }
    else
    {
        // Handle case where appointment does not exist
        // You can throw an exception or return null based on your requirement
        throw new ArgumentException($"Appointment with id {id} not found.");
    }
}

        public async Task<Appointment> UpdateAppointmentStatus(int id, Appointment appointment)
        {
            var oldAppointment = await GetAppointment(id);

            if (oldAppointment != null)
            {
                // Update properties of the existing appointment


                oldAppointment.DateTime = appointment.DateTime;
                oldAppointment.Status = appointment.Status;
                oldAppointment.PatientId = appointment.PatientId;
                oldAppointment.DoctorId = appointment.DoctorId;
                oldAppointment.RecepId = appointment.RecepId;

                try
                {
                    await _dbcontext.SaveChangesAsync();
                    return oldAppointment;
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    Console.WriteLine($"Concurrency conflict occurred: {ex.Message}");
                    throw;
                }
            }
            else
            {
               
                throw new ArgumentException($"Appointment with id {id} not found.");
            }

        }

        public async Task<Appointment> UpdateOnlyOneAppointmentUsingId(int id)
        {
            var oldAppointment = await GetAppointment(id);


            if (oldAppointment != null)
            {
                // Update properties of the existing appointment


                oldAppointment.DateTime = oldAppointment.DateTime;
                oldAppointment.Status = "cancelled";
                oldAppointment.PatientId = oldAppointment.PatientId;
                oldAppointment.DoctorId = oldAppointment.DoctorId;
                oldAppointment.RecepId = oldAppointment.RecepId;

                try
                {
                    var targetPatient=await  GetPatient(oldAppointment.PatientId);

                    var targetEmail = targetPatient.Email;
                    var targetDay = oldAppointment.DateTime.Date;
                    var targetTime = oldAppointment.DateTime;


                    string emailSubject = "Appointment Update: Cancellation Notification";
                    string userName = targetPatient.FullName;
                    string emailMessage = "Dear " + targetPatient.Name + ",\n" + " We regret to inform you that your scheduled appointment with Medicare Hub on " + targetTime + " has been cancelled. We apologize for any inconvenience this may cause you.";


                    EmailSender emailSernder = new EmailSender();
                    await emailSernder.SendMail(emailSubject, targetEmail, userName, emailMessage);

                    await _dbcontext.SaveChangesAsync();

                   



                    return oldAppointment;
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    Console.WriteLine($"Concurrency conflict occurred: {ex.Message}");
                    throw;
                }
            }
            else
            {

                throw new ArgumentException($"Appointment with id {id} not found.");
            }

        }

        public async Task<List<Appointment>> GetAppointmentCountOfDays(int doctorId, int monthId)
        {
            var targetAppointment=_dbcontext.appointments.Where(a=>a.DoctorId==doctorId && (a.DateTime.Month)-1==monthId).ToList();
            return targetAppointment;
        }
    }
}
