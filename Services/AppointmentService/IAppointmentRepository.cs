using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.AppointmentService
{
    public interface IAppointmentRepository
    {
        public Task<List<Appointment>> GetAll();
        public Task AddAppointment(Appointment appointment);

        public Task<Appointment> GetAppointment(int id);

        public Task DeleteAppointment(int id);
        public Task <List<Appointment>> GetDoctorAppointments(int id);

        public Task<List<Appointment>> GetDoctorAppointmentsByDate(int doctorId, DateTime date);

        public Task DeleteAllDoctorDayAppointments(int doctorId, DateTime date);

        public Task<List<User>> GetDoctors();
        public Task<Patient> GetPatient(int id);

        public Task<Appointment> UpdateAppointment(int id,Appointment appointment);

        public Task<List<Patient>> GetPatients();

        public Task RegisterPatient(Patient patient);




    }
}
