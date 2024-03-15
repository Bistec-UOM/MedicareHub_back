using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.DTO.AdminDto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.AdminServices
{

    public class AnalyticsService : IAnalyticsService
    {
        private readonly IRepository<Prescription> _presrepo;
        private readonly ApplicationDbContext _dbcontext;

        public AnalyticsService(IRepository<Prescription> presrepo, ApplicationDbContext dbContext)
        {
            _presrepo = presrepo;
            _dbcontext = dbContext;
        }

        public async Task<List<A_Income>> GetAllAmount()
        {
            var prescriptions = await _presrepo.GetAll();
            var incomes = prescriptions.Select(IncomeMapper.MapToDTO).ToList();
            return incomes;
        }

        public async Task<A_Patient> GetAllPatientDetails()
        {
            var patients = await _dbcontext.patients
                .Select(patient => patient.DOB)
                .ToListAsync();

            var appointmentDates = await _dbcontext.appointments
                .Select(appointment => appointment.DateTime)
                .ToListAsync();

            // Add more details as needed

            var patientDetailsDTO = new A_Patient
            {
                PatientDOBs = patients,
                //AppointmentDates = appointmentDates,
                // Add other details as needed
            };

            return patientDetailsDTO; // Corrected the return statement
        }
    }

}
