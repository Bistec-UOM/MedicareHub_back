using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using Models;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class DoctorappoinmentService
    {
        private readonly IRepository<Appointment> _appoinments;
        private readonly IRepository<Patient> _patients;
        private readonly ApplicationDbContext _context;
        public DoctorappoinmentService(IRepository<Appointment> appoinments, IRepository<Patient> patients, ApplicationDbContext context)
        {
            _appoinments = appoinments;
            _patients = patients;
            _context = context;
        }
        public async Task<List<Object>> GetPatientNamesForApp()
        {
            var tmp= _context.appointments
              
            .Where(a => a.Status == "new") // Filter appointments with status "new"                
            .Select(a => new
            {
                id = a.Id,
                date = a.DateTime.Date,
                time = a.DateTime.TimeOfDay.ToString(@"hh\:mm"),
                status = "pending",
                Patient = new
                {
                    name = a.Patient.Name,
                    age = CaluclateAge((DateTime)a.Patient.DOB),
                    gender = a.Patient.Gender
                }
            })
            .ToList<object>();



            return tmp;
        }

        private static int CaluclateAge(DateTime dob)
        {
            DateTime now = DateTime.UtcNow;
            int age = now.Year - dob.Year;
            if (now.Month < dob.Month || (now.Month == dob.Month && now.Day < dob.Day))
            {
                age--;
            }
            return age;
        }
    }
}