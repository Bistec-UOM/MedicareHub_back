using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.DTO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;


namespace Services.AppointmentService
{
    public class AppointmentService:IAppointmentService
    {

        private readonly ApplicationDbContext _dbcontext;
        private readonly IRepository<Appointment> _appointment;
        private readonly IRepository<Patient> _patient;
        private readonly IRepository<User> _doctor;
        private readonly IRepository<Unable_Date> _unable_date;
        private readonly IRepository<Notification> _notification;
      

        public AppointmentService(ApplicationDbContext dbcontext,IRepository<Appointment> appointment,IRepository<Patient> patient,IRepository<User> doctor,IRepository<Unable_Date> unableDate, IRepository<Notification> notification)
        {
            _dbcontext = dbcontext;
            _appointment = appointment;
            _patient = patient;
            _doctor = doctor;
            _unable_date = unableDate;
            _notification = notification;
        } 
        
        public async Task<int> AddAppointment(Appointment appointment)  //Add an appointment
        {
            bool appointmentExists = _dbcontext.appointments.Any(a => a.PatientId == appointment.PatientId && a.DateTime == appointment.DateTime); //checking if there already appointments for that time slot on that patient
            bool timeBooked=_dbcontext.appointments.Any(a=>a.DoctorId==appointment.DoctorId && ((a.DateTime.AddMinutes(-20)<= appointment.DateTime) && (a.DateTime.AddMinutes(20) >= appointment.DateTime))); //check are there any other appointments for that doctor
            bool timeBlocked = _dbcontext.unable_Dates.Any(a => a.doctorId == appointment.DoctorId && ((appointment.DateTime >= a.StartTime.AddMinutes(-20) && appointment.DateTime<=a.EndTime)));  //checking if the time slot has been blocked by the doctor
            if ((appointmentExists))
            {
                return 1;

            }
            else if(timeBooked)
            {
                return 2;
            }
            else if(timeBlocked)
            {
                return 3;
            }
            else
            {
                await _appointment.Add(appointment);    //adding an appointment using IRepository add function
                var patientId = appointment.PatientId;   // sending an email for the patient
                var patient = await GetPatient(patientId);
                string emailSubject = "Confirmation: Your Appointment with Medicare Hub";
                string userName = patient?.FullName;

                Doctor doctor = _dbcontext.doctors.Find(appointment.DoctorId);
                var doctorUser=_dbcontext.users.Find(doctor.UserId);

                var iconUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRdq0Qw2AUbCppR3IQBWOZx94oZ2NWVuY1vMQ&s";
                var time = appointment.DateTime.ToString("f");
                var venue = "Medicare Hub Clinic, 123 Main Street, City";
                var doctorName = doctorUser?.Name;
                var message = "Dear "+ userName+", <br/><br/>We are delighted to confirm your appointment with Medicare Hub.";

                var htmlContent = $@"
<html>
<body style='font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 20px;'>
    <div style='max-width: 600px; margin: auto; background-color: #ffffff; padding: 20px; border-radius: 10px; box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);'>
        <h1 style='color: #09D636; text-align: center;'>
            <img src='{iconUrl}' style='margin-right: 8px;' width='32' height='32' alt='Hospital Icon'/>
            Medicare <span style='color: #AFDCBB;'>Hub</span>
        </h1>
        <p style='color: #555555; font-size: 16px;'>{message}</p>
        <div style='border: 2px solid #09D636; padding: 15px; border-radius: 10px; background-color: #e8f5e9;'>
            <h3 style='color: #09D636;'>Appointment Details</h3>
            <ul style='color: #555555; font-size: 16px;'>
                <li><strong>Time:</strong> {time}</li>
                <li><strong>Venue:</strong> {venue}</li>
                <li><strong>Doctor:</strong> {doctorName}</li>
            </ul>
        </div>
          <p style='font-size: 16px; color: #555;'>
            <span >Our team looks forward to providing you with exceptional care and service.</span> 
            <br/><br/>
            If you have any questions or need further assistance, 
            <span>please do not hesitate to contact us</span>. 
            <br/><br/>
            <span>Thank you for choosing Medicare Hub!.</span>
        </p>
        <p style='font-size: 16px; color: #555;'>
            Best regards,
            <br/>
            <span style='color: #007BFF;'>Medicare Hub Team</span>
        </p>
    </div>
</body>
</html>";

                string pemail;
                if(patient!=null)
                {
                    pemail= patient.Email;
                }
                else
                {
                    pemail = "abc@gmail.com";
                }
               
                EmailSender emailSernder = new EmailSender();
                await emailSernder.SendMail(emailSubject, pemail, userName, htmlContent);


                return 0;   

            }
   
        }
        public async Task<List<Appointment>> CancelAllAppointments(int doctorId, DateTime date)  //services function for cancelling the all appointments by a doctor
        {
            var appointments = await GetDoctorAppointmentsByDate(doctorId, date);
            List<Appointment> updatedResults = new List<Appointment>();

            foreach (Appointment app in appointments)  //filter new appointmetns and change the status and add them to the updatedResults list
            {
                if (app.Status == "new") // check the status of an appointment.because cancel is allowed only for new appointments
                {
                    var upAppointment = await UpdateOnlyOneAppointmentUsingId(app.Id);  //change the status of a new appointment
                    updatedResults.Add(upAppointment);
                }
            }
            return updatedResults;
        }
        public async Task<List<Appointment>> DeleteAllDoctorDayAppointments(int doctorId, DateTime date)  //service function for real delete  of new  appointments from table
        {
            var targetAppointments = await GetDoctorAppointmentsByDate(doctorId, date);

            if (targetAppointments != null && targetAppointments.Any())
            {
                // Filter appointments with status "new"
                var appointmentsToDelete = targetAppointments.Where(appointment => appointment.Status == "new").ToList();

                if (appointmentsToDelete.Any())
                {
                    _dbcontext.appointments.RemoveRange(appointmentsToDelete);  //remove the appointmentsToDelete list from table
                    _dbcontext.SaveChanges();
                   
                }
                return appointmentsToDelete;

            }
            else
            {
                return new List<Appointment>();  //return emply list if there are no any app to delete
            }
        }

        public async Task<Patient> GetPatient(int id)  //Get patient by id
        {
            var patient = _patient.Get(id);
            return await patient;
        }

        public async Task<Appointment> DeleteAppointment(int id)  //delete appointment by id
        {
            var appointmentk=await GetAppointment(id);
            if (appointmentk != null)
            {
               await _appointment.Delete(id);
                return appointmentk;

            }
            else
            {
                throw new ArgumentException($"Appointment with id {id} not found.");
            }
        }

        public  async Task<List<Appointment>> GetAll()  //get all appointments
        {
            return await _appointment.GetAll();     
        }
        public async Task<Appointment> GetAppointment(int id)  //get one appointment by id
        {
            var appointment = _appointment.Get(id);
            return await appointment;
        }
        public async Task<List<Appointment>> GetDoctorAppointments(int doctorId)   //getting all the appointments of a specific doctor
        {   
            var doctorAppointments =  _dbcontext.appointments.Where(a => a.DoctorId == doctorId);
            return doctorAppointments.ToList();
        }

       
        public async Task<List<Appointment>> GetDoctorAppointmentsByDate(int doctorId, DateTime date)   //getting all the appointments of a specific doctor of a specific date
        {
            var doctorDayAppointments =  _dbcontext.appointments.Where(a => a.DoctorId == doctorId && a.DateTime.Date == date);
            return doctorDayAppointments.ToList();   
        }

        public async Task<List<AppointmentWithPatientDetails>> GetDoctorAppointmentsByDateWithPatientDetails(int doctorId, DateTime date)  //getting doctor apps of a date with patient details
        {
            var doctorDayAppointments = await GetDoctorAppointmentsByDate(doctorId, date);
            List<AppointmentWithPatientDetails> appointmentsWithDetails = new List<AppointmentWithPatientDetails>();
            foreach (var appointment in doctorDayAppointments)   //returning the appointment details as well as patient details of the relevent appointment
            {
                var patientDetails = await GetPatient(appointment.PatientId);
                AppointmentWithPatientDetails newappointment = new AppointmentWithPatientDetails
                {
                    Appointment = appointment,
                    patient = patientDetails

                };

                appointmentsWithDetails.Add(newappointment);
            }
            return appointmentsWithDetails;
        }



        public async Task<List<User>> GetDoctors()  //getting the doctors list
        {
            var doctors =  _dbcontext.users.Where(d => d.Role == "Doctor");
            return  doctors.ToList();      
        }
        public async Task<List<Patient>> GetPatients()  //getting the patient list
        {
           
            return  await _patient.GetAll();
        }
        public async Task RegisterPatient(Patient patient)  //registering a patient
        {
            await _patient.Add(patient);  // Adding the patient to the table
            string emailSubject = "Your Medicare Hub Membership: A Warm Welcome Awaits!";
            string userName = patient.FullName;
            var iconUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRdq0Qw2AUbCppR3IQBWOZx94oZ2NWVuY1vMQ&s";
            string emailMessage = "Dear " + patient.Name + ",<br/><br/>Thank you for choosing Medicare Hub. We're honored to be part of your healthcare journey. Please reach out if you need anything.";

            var htmlContent = $@"
<html>
<body style='font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 20px;'>
    <div style='max-width: 600px; margin: auto; background-color: #ffffff; padding: 20px; border-radius: 10px; box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);'>
        <h1 style='color: #09D636; text-align: center;'>
            <img src='{iconUrl}' style='margin-right: 8px;' width='32' height='32' alt='Hospital Icon'/>
            Medicare <span style='color: #AFDCBB;'>Hub</span>
        </h1>
        <p style='color: #555555; font-size: 16px;'>{emailMessage}</p>
        <div style='border: 2px solid #09D636; padding: 15px; border-radius: 10px; background-color: #e8f5e9;'>
            <h3 style='color: #09D636; text-align: center;'>Welcome to Medicare Hub</h3>
            <p style='color: #555555; font-size: 16px;'>
                We are delighted to have you as a member of our community. At Medicare Hub, we are committed to providing you with exceptional care and service. Here are some of the benefits you can look forward to:
            </p>
            <ul style='color: #555555; font-size: 16px;'>
                <li>Access to top-notch medical professionals</li>
                <li>Comprehensive healthcare services</li>
                <li>Personalized care plans</li>
                <li>24/7 customer support</li>
            </ul>
        </div>
        <p style='font-size: 16px; color: #555;'>
            <span>Our team looks forward to providing you with exceptional care and service.</span> 
            <br/><br/>
            If you have any questions or need further assistance, 
            <span>please do not hesitate to contact us</span>. 
            <br/><br/>
            <span >Thank you for choosing Medicare Hub!</span>
        </p>
        <p style='font-size: 16px; color: #555;'>
            Best regards,
            <br/>
            <span style='color: #007BFF;'>Medicare Hub Team</span>
        </p>
    </div>
</body>
</html>";



            EmailSender emailSernder = new EmailSender();  //sending email after succeful registration of a patient
            await emailSernder.SendMail(emailSubject,patient.Email,userName,htmlContent);
        }
       public async Task<int> UpdateAppointment(int id, Appointment appointment)  //just update appointment(time)
        {   
            var oldAppointment = await GetAppointment(id);

             
            if (oldAppointment != null)
            {
                bool timeBooked = _dbcontext.appointments.Any(a => a.DoctorId == appointment.DoctorId &&  ((a.DateTime.AddMinutes(-20) <= appointment.DateTime) && (a.DateTime.AddMinutes(20) >= appointment.DateTime))); //check are there any other appointments for that doctor
                bool timeBlocked = _dbcontext.unable_Dates.Any(a => a.doctorId == appointment.DoctorId && ((appointment.DateTime >= a.StartTime.AddMinutes(-20) && appointment.DateTime <= a.EndTime)));  //check the selected time slot has been blocked

                oldAppointment.DateTime = appointment.DateTime;    // Update properties of the existing appointment
                oldAppointment.Status = appointment.Status;
                oldAppointment.PatientId = appointment.PatientId;
                oldAppointment.DoctorId = appointment.DoctorId;
                oldAppointment.RecepId = appointment.RecepId;
                if(timeBooked)
                {
                    return 1;
                }
                else if(timeBlocked)
                {
                    return 2;
                }
                else
                {
                    try
                    {

                        await _dbcontext.SaveChangesAsync();
                        var patientId = appointment.PatientId;   // sending an email to the patient
                        var patient = await GetPatient(patientId);
                        string emailSubject = "Appointment Update: Your Updated Appointment with Medicare Hub";
                        string userName = patient?.FullName;

                        Doctor doctor = _dbcontext.doctors.Find(appointment.DoctorId);
                        var doctorUser = _dbcontext.users.Find(doctor.UserId);

                        var iconUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRdq0Qw2AUbCppR3IQBWOZx94oZ2NWVuY1vMQ&s";
                        var newTime = appointment.DateTime.ToString("f");  // Formatted date and time
                        var venue = "Medicare Hub Clinic, 123 Main Street, City";
                        var doctorName = doctorUser.Name;
                        var message = $"Dear {userName},<br/><br/>We would like to inform you that your appointment with Medicare Hub has been rescheduled. Please find the updated details below:";

                        var htmlContent = $@"
<html>
<body style='font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 20px;'>
    <div style='max-width: 600px; margin: auto; background-color: #ffffff; padding: 20px; border-radius: 10px; box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);'>
        <h1 style='color: #09D636; text-align: center;'>
            <img src='{iconUrl}' style='margin-right: 8px;' width='32' height='32' alt='Hospital Icon'/>
            Medicare <span style='color: #AFDCBB;'>Hub</span>
        </h1>
        <p style='color: #555555; font-size: 16px;'>{message}</p>
        <div style='border: 2px solid #09D636; padding: 15px; border-radius: 10px; background-color: #e8f5e9;'>
            <h3 style='color: #09D636;'>Updated Appointment Details</h3>
            <ul style='color: #555555; font-size: 16px;'>
                <li><strong>New Time:</strong> {newTime}</li>
                <li><strong>Venue:</strong> {venue}</li>
                <li><strong>Doctor:</strong> {doctorName}</li>
            </ul>
        </div>
        <p style='font-size: 16px; color: #555;'>
            <span>Our team looks forward to providing you with exceptional care and service.</span> 
            <br/><br/>
            If you have any questions or need further assistance, 
            <span >please do not hesitate to contact us</span>. 
            <br/><br/>
            <span >Thank you for choosing Medicare Hub!</span>
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
                        await emailSernder.SendMail(emailSubject, patient.Email, userName, htmlContent);
                        return 0;
                    }
                    catch (Exception ex)
                    {

                        throw new Exception("Error occured during the updation");
                    }

                }
                 
            }
            else
            {
                throw new ArgumentException($"Appointment with id {id} not found.");  //throw an exception if the appointment does not exist
            }
        }
        public async Task<Appointment> UpdateAppointmentStatus(int id, Appointment appointment)  //update  appointment status
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
                  
                    throw new Exception("Error occured during the updation");
                }
            }
            else
            {
               
                throw new ArgumentException($"Appointment with id {id} not found.");
            }

        }
        public async Task<Appointment> UpdateOnlyOneAppointmentUsingId(int id)  //update only the appointment status from new to cancelled
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
                    var targetTime = oldAppointment.DateTime.ToString("f");
                    string emailSubject = "Appointment Update: Cancellation Notification"; // Sending the cancel notification mail
                    string userName = targetPatient.FullName;
                    var iconUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRdq0Qw2AUbCppR3IQBWOZx94oZ2NWVuY1vMQ&s";
                    string emailMessage = "Dear " + targetPatient.Name + ",<br/><br/> We regret to inform you that your scheduled appointment with Medicare Hub on " + targetTime + " has been cancelled. We apologize for any inconvenience this may cause you.";

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
            <span>Our team looks forward to providing you with exceptional care and service.</span> 
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

                    await _dbcontext.SaveChangesAsync();
                    return oldAppointment;
                }
                catch (Exception ex)
                {
                    
                    throw new Exception("Error occured during the updation");
                }
            }
            else
            {

                throw new ArgumentException($"Appointment with id {id} not found.");
            }

        }
        public async Task<List<Appointment>> GetAppointmentCountOfDays(int doctorId, int monthId)  //get monthly appointments  of a doctor for progress bar
        {
            var targetAppointment=_dbcontext.appointments.Where(a=>a.DoctorId==doctorId && (a.DateTime.Month)-1==monthId).ToList();
            return targetAppointment;
        }
        public async Task AddUnableDate(Unable_Date uDate) //adding blocking dates
        {
            await _unable_date.Add(uDate);
        }
        public async Task<List<Unable_Date>> getUnableDates(int doctorId)  //getting blocking dates of a specific doctor
        {
            var uDates = _dbcontext.unable_Dates.Where(u => u.doctorId == doctorId && u.StartTime.Hour==00 && u.StartTime.Minute==00&& u.EndTime.Hour==23 && u.EndTime.Minute==59);
            return  uDates.ToList();
        }

        public async  Task AddNotification(Notification notification)
        {
             await _notification.Add(notification);  
           

        }

        public async Task<List<Notification>> getNotifications(int userId)  //getting only unseen notifications or todays notifications
        {

            var today = DateTime.Today;



            var notifications = _dbcontext.notification
    .Where(u => u.To == userId.ToString() &&
                ((!u.Seen.HasValue || !u.Seen.Value) ||
                 (u.SendAt != null && u.SendAt.Value.Date == today)))
    .ToList();


            return notifications.ToList();

        }

        public async Task markAsSeenNotifications(int userId, bool newSeenValue)
        {
            var notifications = _dbcontext.notification.Where(u => u.To == userId.ToString() && u.Seen==false);
            foreach (var notification in notifications)
            {
                notification.Seen = newSeenValue;
            }


            await _dbcontext.SaveChangesAsync();

        }

        public async Task<List<Unable_Date>> getUnableTimeslots(int doctorId,DateTime day)
        {
            var dates= _dbcontext.unable_Dates.Where(u => u.doctorId == doctorId && u.Date == day);
            return dates.ToList();

        }

       
      

        
    }
}
