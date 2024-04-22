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
            var check = await _dbcontext.users
                .Include(u => u.User_Tele)
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
                    .GroupBy(bd => new { bd.DrugID, bd.Drug.GenericN, bd.Drug.Weight });

                // Calculate total drug usage for each drug on this date
                foreach (var drugGroup in drugUsageByDrug)
                {
                    var drugID = drugGroup.Key.DrugID;
                    var namefor = drugGroup.Key.GenericN + "(" + drugGroup.Key.Weight + " mg)";
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
                .GroupBy(p => new { p.CashierId, Month = p.DateTime.Month, Year = p.DateTime.Year })
                .Select(g => new
                {
                    cashierId = g.Key.CashierId,
                    cashierName = g.First().Cashier.User.Name,
                    cashierMonth = g.Key.Year.ToString() + "-" + g.Key.Month.ToString(),
                    Count = g.Select(p => p.DateTime.Date).Distinct().Count()
                })
                .ToListAsync();

            var recep_attendance = await _dbcontext.appointments
                .Include(ap => ap.Recep)
                .ThenInclude(r => r.User)
                // Grouping by RecepId and extracting month from DateTime
                .GroupBy(ap => new { ap.RecepId, Month = ap.DateTime.Month, Year = ap.DateTime.Year })
                .Select(g => new
                {
                    recepId = g.Key.RecepId,
                    recepName = g.First().Recep.User.Name,
                    recepMonth = g.Key.Year.ToString() + "-" + g.Key.Month.ToString(),
                    Count = g.Select(p => p.DateTime.Date).Distinct().Count()
                })
                .ToListAsync();

            var labrep_attendance = await _dbcontext.labReports
                .Include(lr => lr.LbAst)
                .ThenInclude(la => la.User)
                // Grouping by LbAstID and extracting month from DateTime
                .GroupBy(lr => new { lr.LbAstID, Month = lr.DateTime.HasValue ? lr.DateTime.Value.Month : (int?)null, Year = lr.DateTime.HasValue ? lr.DateTime.Value.Year : (int?)null })
                .Select(g => new
                {
                    labAstId = g.Key.LbAstID,
                    labAstName = g.First().LbAst.User.Name,
                    labAstDate = g.Key.Year.ToString() + "-" + g.Key.Month.ToString(),
                    // Changed to extract date instead of month for counting
                    Count = g.Select(p => p.DateTime).Where(d => d.HasValue).Select(d => d.Value.Date).Distinct().Count()
                })
                .ToListAsync();
            var doct_attendance = await _dbcontext.prescriptions
                .Include(p => p.Appointment)
                    .ThenInclude(ap => ap.Doctor)
                        .ThenInclude(d => d.User)
                .Where(p => p.Appointment.Status == "Completed")
                .GroupBy(p => new { p.Appointment.DoctorId, Year = p.DateTime.Year, Month = p.DateTime.Month })
                .Select(g => new
                {
                    doctId = g.Key.DoctorId,
                    doctName = g.First().Appointment.Doctor.User.Name,
                    // Concatenate year and month as a string
                    doctDate = g.Key.Year.ToString() + "-" + g.Key.Month.ToString(),
                    count = g.Select(p => p.DateTime.Date).Distinct().Count()
                })
                .ToListAsync();




            total_attendance.Add(new { c_at = cashier_attendance, r_at = recep_attendance, l_at = labrep_attendance, d_at = doct_attendance });

            return total_attendance;
        }
        public async Task<object> GetUsers()
        {
            var users = await _dbcontext.users
                .Select(u => new { u.Id, u.Name, u.Role })
                .ToListAsync();

            return users.Select(u => new { id = u.Id, name = u.Name, role = u.Role }).ToList();

        }
        public async Task<object> CheckAttendance(DateTime date)
        {
            var total_attendance = new List<object>();

            var cashier_attendance = await _dbcontext.prescriptions
                .Include(p => p.Cashier)
                .ThenInclude(c => c.User)
                .Where(p => p.DateTime.Year == date.Year && p.DateTime.Month == date.Month)
                .GroupBy(p => new { p.CashierId })
                .Select(g => new
                {
                    Id = g.First().Cashier.User.Id,
                    Name = g.First().Cashier.User.Name,
                    Role = "Cashier",
                    Count = g.Select(p => p.DateTime.Date).Distinct().Count()
                })
                .ToListAsync();

            var recep_attendance = await _dbcontext.appointments
                .Include(ap => ap.Recep)
                .ThenInclude(r => r.User)
                .Where(p => p.DateTime.Year == date.Year && p.DateTime.Month == date.Month)
                .GroupBy(ap => new { ap.RecepId })
                .Select(g => new
                {
                    Id = g.First().Recep.User.Id,
                    Name = g.First().Recep.User.Name,
                    Role = "Receptionist",
                    Count = g.Select(ap => ap.DateTime.Date).Distinct().Count()
                })
                .ToListAsync();

            var labrep_attendance = await _dbcontext.labReports
                .Include(lr => lr.LbAst)
                .ThenInclude(la => la.User)
                .Where(p => p.DateTime.Value.Year == date.Year && p.DateTime.Value.Month == date.Month)
                .GroupBy(lr => new { lr.LbAstID })
                .Select(g => new
                {
                    Id = g.First().LbAst.User.Id,
                    Name = g.First().LbAst.User.Name,
                    Role = "Lab Assistant",
                    Count = g.Select(p => p.DateTime).Where(d => d.HasValue).Select(d => d.Value.Date).Distinct().Count()
                })
                .ToListAsync();

            var doct_attendance = await _dbcontext.prescriptions
                .Include(p => p.Appointment)
                .ThenInclude(ap => ap.Doctor)
                .ThenInclude(d => d.User)
                .Where(p => p.Appointment.Status == "Completed" && p.Appointment.DateTime.Year == date.Year && p.Appointment.DateTime.Month == date.Month)
                .GroupBy(p => new { p.Appointment.DoctorId })
                .Select(g => new
                {
                    Id = g.First().Appointment.Doctor.User.Id,
                    Name = g.First().Appointment.Doctor.User.Name,
                    Role = "Doctor",
                    Count = g.Select(p => p.DateTime.Date).Distinct().Count()
                })
                .ToListAsync();

            //take all business days
            var total_days = await _dbcontext.appointments
                .Where(p => p.DateTime.Year == date.Year && p.DateTime.Month == date.Month)
                .GroupBy(p => new { p.DateTime.Month})
                .Select(g => new
                {
                    WorkingDayCount = g.Select(p => p.DateTime.Date).Distinct().Count()
                })
                .ToListAsync();

            // Cast each int to object before adding to total_attendance
            total_attendance.AddRange(total_days);
            total_attendance.AddRange(cashier_attendance);
            total_attendance.AddRange(recep_attendance);
            total_attendance.AddRange(labrep_attendance);
            total_attendance.AddRange(doct_attendance);


            return total_attendance;
        }
        public async Task<object> GetLabReports()
        {
            var TotalLabReports = new List<object>();
            var LabReports = await _dbcontext.labReports
                .Include(lr => lr.Prescription)
                .Include(lr => lr.Test)
                .GroupBy(lr => lr.Prescription.DateTime.Date)
                .ToListAsync();
            foreach (var group in LabReports)
            {
                var LabReportsForDay = new List<object>();
                var Date = group.Key.Date;
                var LabReportsByType = new List<object>();
                var ReportsByType = group
                    .GroupBy(g => g.TestId);
                foreach (var sub in ReportsByType)
                {
                    var TypeId = sub.First().Id;
                    var TypeName = sub.First().Test.TestName;
                    var count = sub.Count();

                    LabReportsForDay.Add(new { Id = TypeId, name = TypeName, count = count });
                }
                TotalLabReports.Add(new { date = Date, reports = LabReportsForDay });
            }


            return TotalLabReports;
        }





    }
}

