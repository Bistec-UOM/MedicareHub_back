using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using Models;
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
            
        }

        public async Task DeleteAllDoctorDayAppointments(int doctorId, DateTime date)
        {
            var targetAppointments=await GetDoctorAppointmentsByDate(doctorId, date);

            if (targetAppointments != null && targetAppointments.Any())
            {
                _dbcontext.appointments.RemoveRange(targetAppointments);
                await _dbcontext.SaveChangesAsync();
            }

        }

        public async Task DeleteAppointment(int id)
        {
            var appointment=await GetAppointment(id);
           _dbcontext.appointments.Remove(appointment);
           _dbcontext.SaveChanges();
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
            var doctorDayAppointments =  _dbcontext.appointments.Where(a => a.Id == doctorId && a.Time.Date == date);
            return doctorDayAppointments.ToList();  
            
        }

        public async Task<List<User>> GetDoctors()
        {

            var doctors = _dbcontext.users.Where(d => d.Role == "Doctor");
            return doctors.ToList();
           
        }

        public async Task<User> GetPatient(int id)
        {
            var patient=_dbcontext.users.FirstOrDefaultAsync(p=>p.Id==id);
            return await patient;
        }

        public async Task<List<User>> GetPatients()
        {
            var patients = _dbcontext.users.Where(p => p.Role == "Patient");
            return patients.ToList();
        }

        public async Task<Appointment> UpdateAppointment(int id, Appointment appointment)


{

            System.Diagnostics.Debug.WriteLine("inside update");
            System.Diagnostics.Debug.WriteLine("newtime", appointment.Time.ToString(),appointment.DoctorId);
            var oldAppointment = await GetAppointment(id);

    if (oldAppointment != null)
    {
                // Update properties of the existing appointment

                
            oldAppointment.Time = appointment.Time;
        oldAppointment.Status = appointment.Status;
        oldAppointment.PatitenId = appointment.PatitenId;
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


        
    }
}
