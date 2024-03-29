using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Services.AdminServices
{

    public class AnalyticsService : IAnalyticsService
    {
        private readonly ApplicationDbContext _dbcontext;
        public AnalyticsService(ApplicationDbContext dbContext)
        {
            _dbcontext = dbContext;
        }

        public async Task<object> GetMaleFemalePatientsCountAllDays()
        {
            var Prescriptions = await _dbcontext.prescriptions
                .Include(ap => ap.Appointment)
                .ThenInclude(a => a.Patient)
                .ToListAsync();

            var countsByDay = new List<object>();

            // Extract distinct dates
            var distinctDates = Prescriptions.Select(p => p.DateTime.Date).Distinct();
            foreach (var date in distinctDates)
            {
                var child_male = Prescriptions
                   .Where(p => p.DateTime.Date == date)
                   .Select(p => p.Appointment)
                   .Count(a => a?.Patient?.Gender == "Male" && CalculateAge(a.Patient.DOB) < 18);
                var child_female = Prescriptions
                    .Where(p => p.DateTime.Date == date)
                    .Select(p => p.Appointment)
                    .Count(a => a?.Patient?.Gender == "Female" && CalculateAge(a.Patient.DOB) < 18);

                var adult_male = Prescriptions
                    .Where(p => p.DateTime.Date == date)
                    .Select(p => p.Appointment)
                    .Count(a => a?.Patient?.Gender == "Male" && CalculateAge(a.Patient.DOB) >= 18 && CalculateAge(a.Patient.DOB) < 45);
                var adult_female = Prescriptions
                    .Where(p => p.DateTime.Date == date)
                    .Select(p => p.Appointment)
                    .Count(a => a?.Patient?.Gender == "Female" && CalculateAge(a.Patient.DOB) >= 18 && CalculateAge(a.Patient.DOB) < 45);

                var old_male = Prescriptions
                    .Where(p => p.DateTime.Date == date)
                    .Select(p => p.Appointment)
                    .Count(a => a?.Patient?.Gender == "Male" && CalculateAge(a.Patient.DOB) > 45);
                var old_female = Prescriptions
                    .Where(p => p.DateTime.Date == date)
                    .Select(p => p.Appointment)
                    .Count(a => a?.Patient?.Gender == "Female" && CalculateAge(a.Patient.DOB) > 45);


                countsByDay.Add(new
                {
                    datefor = date,
                    child_male = child_male,
                    child_female = child_female,
                    adult_male = adult_male,
                    adult_female = adult_female,
                    old_male = old_male,
                    old_female = old_female,
                });
            }

            return countsByDay;
        }
        private int CalculateAge(DateTime? dob)
        {   //hasvalue use for not nullable values :- datetime?
            if (dob.HasValue)
            {
                DateTime now = DateTime.Today;
                int age = now.Year - dob.Value.Year;

                // If the current date is before the birth date of this year's birthday, decrement the age
                // This adjustment is necessary because if the birthday hasn't occurred yet this year,
                // the age should be one less than the difference in years between birth and current date
                if (now < dob.Value.AddYears(age))
                {
                    age--;
                }
                return age;
            }
            return 0; // Return 0 if date of birth is not valid
        }
        public async Task<object> GetTotalAmount()
        {
            var totalByDay = new List<object>();

            // Retrieve all prescriptions with their associated bill_drug records
            var prescriptions = await _dbcontext.prescriptions
                .Include(p => p.Bill_drug)
                .ToListAsync();

            //group them by date
            var prescriptBydate = prescriptions
                .GroupBy(p => p.DateTime.Date);
            foreach (var grp in prescriptBydate)
            {
                var date = grp.Key;
                var totalAmount = 0;

                foreach (var prescript in grp)
                {
                    totalAmount += prescript.Bill_drug.Sum(bd => bd.Amount);
                }
                // Add the date and total amount to the result list
                totalByDay.Add(new { datefor = date, income = totalAmount });
            }
            return totalByDay;
        }
        public async Task<object> GetAvailableCount()
        {
            // Query the database to get the generic name and available count of 
            var drugAvailability = _dbcontext.drugs
                .Select(d => new
                {
                    Name = d.BrandN + "(" + (d.Weight) + "mg)",
                    Available = d.Avaliable
                })
                .ToList();

            return drugAvailability;

        }
        public async Task<object> GetTotalDrugUsage()
        {
            var totalDrugUsage = new List<object>();

            // Retrieve all prescriptions with their associated bill_drug records
            var prescriptions = await _dbcontext.prescriptions
                .Include(p => p.Bill_drug)
                .ThenInclude(bd => bd.Drug)
                .ToListAsync();

            // Group bill_drug records by Date
            var billDrugGroupsByDate = prescriptions
                .SelectMany(p => p.Bill_drug)
                .GroupBy(bd => bd.Prescription.DateTime.Date);

            // Calculate total drug usage for each drug on each date
            foreach (var group in billDrugGroupsByDate)
            {
                var date = group.Key.Date;
                var drugUsageForDate = new List<object>();

                // Group drug usage for this date by DrugID and DrugName
                var drugUsageByDrug = group
                    .GroupBy(bd => new { bd.DrugID, bd.Drug.GenericN,bd.Drug.Weight });

                // Calculate total drug usage for each drug on this date
                foreach (var drugGroup in drugUsageByDrug)
                {
                    var drugID = drugGroup.Key.DrugID;
                    var namefor = drugGroup.Key.GenericN+"("+ drugGroup.Key.Weight+ " mg)";
                    var amount = drugGroup.Sum(bd => bd.Amount);

                    drugUsageForDate.Add(new { name = namefor, quantity = amount });
                }

                // Add drug usage for this date to the output list
                totalDrugUsage.Add(new { datefor = date.Date, drugType = drugUsageForDate });
            }

            return totalDrugUsage;
        }
        public async Task<object> GetAttendance()
        {
            var total_attendance = new List<object>();

            var cashier_attendance = await _dbcontext.prescriptions
                .Include(p => p.Cashier)
                .ThenInclude(c => c.User)
                // Grouping by CashierId and extracting month from DateTime
                .GroupBy(p => new { p.CashierId, Month = p.DateTime.Month })
                .Select(g => new
                {
                    cashierId = g.Key.CashierId,
                    cashierName = g.First().Cashier.User.Name,
                    cashierMonth = g.Key.Month,
                    Count = g.Select(p => p.DateTime.Date).Distinct().Count()
                })
                .ToListAsync();

            var recep_attendance = await _dbcontext.appointments
                .Include(ap => ap.Recep)
                .ThenInclude(r => r.User)
                // Grouping by RecepId and extracting month from DateTime
                .GroupBy(ap => new { ap.RecepId, Month = ap.DateTime.Month })
                .Select(g => new
                {
                    recepId = g.Key.RecepId,
                    recepName = g.First().Recep.User.Name,
                    recepMonth = g.Key.Month,
                    Count = g.Select(p => p.DateTime.Date).Distinct().Count()
                })
                .ToListAsync();

            var labrep_attendance = await _dbcontext.labReports
                .Include(lr => lr.LbAst)
                .ThenInclude(la => la.User)
                // Grouping by LbAstID and extracting month from DateTime
                .GroupBy(lr => new { lr.LbAstID, Month = lr.DateTime.HasValue ? lr.DateTime.Value.Month : (int?)null })
                .Select(g => new
                {
                    labAstId = g.Key.LbAstID,
                    labAstName = g.First().LbAst.User.Name,
                    labAstMonth = g.Key.Month,
                    // Changed to extract date instead of month for counting
                    Count = g.Select(p => p.DateTime).Where(d => d.HasValue).Select(d => d.Value.Date).Distinct().Count()
                })
                .ToListAsync();



            total_attendance.Add(new { c_at = cashier_attendance, r_at = recep_attendance, l_at = labrep_attendance });

            return total_attendance;
        }

    }
}

