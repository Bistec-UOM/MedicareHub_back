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
        private readonly ApplicationDbContext? _dbcontext;

        public async Task<object> GetMaleFemalePatientsCountAllDays()
        {
            var Prescriptions = await _dbcontext.prescriptions
                .Include(p => p.Appointment)
                .ThenInclude(a => a.Patient)
                .ToListAsync();

            var countsByDay = new List<object>();

            // Extract distinct dates
            var distinctDates = Prescriptions.Select(p => p.DateTime.Date).Distinct();
            foreach (var date in distinctDates) 
            {
                var maleCount = Prescriptions
                   .Where(p => p.DateTime.Date == date)
                   .Select(p => p.Appointment)
                   .Count(a => a?.Patient?.Gender == "Male");

                var femaleCount = Prescriptions
                    .Where(p => p.DateTime.Date == date)
                    .Select(p => p.Appointment)
                    .Count(a => a?.Patient?.Gender == "Female");
                countsByDay.Add(new {Date=date,MaleCount=maleCount, FemaleCount=femaleCount});
            }

            return countsByDay;
        }
    }

}
