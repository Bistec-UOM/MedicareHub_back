using Microsoft.EntityFrameworkCore;
using Models.DTO;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.AppointmentService
{
    public interface IAppointmentService
    {
        public Task<int> AddAppointment(Appointment appointment); //Add an appointment


        public Task<List<Appointment>> CancelAllAppointments(int doctorId, DateTime date);  //services function for cancelling the all appointments by a doctor

        public  Task<List<Appointment>> DeleteAllDoctorDayAppointments(int doctorId, DateTime date);  //service function for real delete  of new  appointments from table


        public Task<Patient> GetPatient(int id); //Get patient by id


        public  Task<Appointment> DeleteAppointment(int id);  //delete appointment by id

        public Task<List<Appointment>> GetAll();  //get all appointments

        public  Task<Appointment> GetAppointment(int id);  //get one appointment by id

        public Task<List<Appointment>> GetDoctorAppointments(int doctorId);  //getting all the appointments of a specific doctor



        public Task<List<Appointment>> GetDoctorAppointmentsByDate(int doctorId, DateTime date);   //getting all the appointments of a specific doctor of a specific date


        public Task<List<AppointmentWithPatientDetails>> GetDoctorAppointmentsByDateWithPatientDetails(int doctorId, DateTime date);  //getting doctor apps of a date with patient details




        public  Task<List<User>> GetDoctors();  //getting the doctors list
        public  Task<List<Patient>> GetPatients();  //getting the patient list

        public Task RegisterPatient(Patient patient);  //registering a patient

        public Task<int> UpdateAppointment(int id, Appointment appointment);  //just update appointment(time)


        public Task<Appointment> UpdateAppointmentStatus(int id, Appointment appointment);  //update  appointment status

        public Task<Appointment> UpdateOnlyOneAppointmentUsingId(int id);  //update only the appointment status from new to cancelled

        public Task<List<Appointment>> GetAppointmentCountOfDays(int doctorId, int monthId);  //get monthly appointments  of a doctor for progress bar


        public Task AddUnableDate(Unable_Date uDate); //adding blocking dates


        public Task<List<Unable_Date>> getUnableDates(int doctorId);  //getting blocking dates of a specific doctor



        public Task AddNotification(Notification notification);



        public Task<List<Notification>> getNotifications(int userId);  //getting only unseen notifications or todays notifications


        public Task markAsSeenNotifications(int userId, bool newSeenValue);



        public Task<List<Unable_Date>> getUnableTimeslots(int doctorId, DateTime day);
        public  Task<Unable_Date> UnblockDay(int id);


    }
}
