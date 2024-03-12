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
        private readonly IRepository<Prescription> _presrepo; // Assuming IRepository<Prescription> is correct
        private readonly ApplicationDbContext _dbcontext;
        public AnalyticsService(IRepository<Prescription> presrepo,ApplicationDbContext dbContext)
        {
            _presrepo = presrepo;
            _dbcontext = dbContext;
        }

        public async Task<List<A_Income>> GetAllAmount()
        {
            // Assuming IncomeMapper.MapToDTO is a method to map Prescription to Income
            var prescriptions = await _presrepo.GetAll();
            var incomes = prescriptions.Select(IncomeMapper.MapToDTO).ToList();
            return incomes;
        }
        public async Task<(List<string> PatientDOBs, List<DateTime> AppointmentDates)> GetAllPatientDetails()
        {
            var patients = await _dbcontext.patients
                .Select(patients=>patients.DOB)
                .ToListAsync();
            var appointDate = await _dbcontext.appointments
                .Select(appointDate=>appointDate.DateTime)
                .ToListAsync();

            return (patients, appointDate);
        }
        
    }
}
