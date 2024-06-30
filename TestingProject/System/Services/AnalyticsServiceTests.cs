using Xunit;
using Services.AdminServices;
using DataAccessLayer;
using Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace TestingProject.System.Services
{
    public class AnalyticsServiceTests
    {
        private readonly AnalyticsService _service;
        private readonly ApplicationDbContext _dbContext;

       

            public AnalyticsServiceTests()
            {
                var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(databaseName: "TestDatabaseNew")
                    .Options;

                _dbContext = new ApplicationDbContext(options);
                _service = new AnalyticsService(_dbContext);

                SeedDatabase();
            }

            private void SeedDatabase()
            {
                // Clear existing data to prevent key conflicts
                _dbContext.labReports.RemoveRange(_dbContext.labReports.ToList());
                _dbContext.prescriptions.RemoveRange(_dbContext.prescriptions.ToList());
                _dbContext.appointments.RemoveRange(_dbContext.appointments.ToList());
                _dbContext.patients.RemoveRange(_dbContext.patients.ToList());
                _dbContext.users.RemoveRange(_dbContext.users.ToList());
                _dbContext.labAssistants.RemoveRange(_dbContext.labAssistants.ToList());
                _dbContext.tests.RemoveRange(_dbContext.tests.ToList());
                _dbContext.drugs.RemoveRange(_dbContext.drugs.ToList());  // Clear drugs
                _dbContext.SaveChanges();

                // Add data to the in-memory database for testing
                _dbContext.patients.AddRange(new List<Patient>
            {
                new Patient { Id = 1, Name = "John Doe", Gender = "Male", DOB = DateTime.Now.AddYears(-10) },
                new Patient { Id = 2, Name = "Jane Doe", Gender = "Female", DOB = DateTime.Now.AddYears(-10) },
                new Patient { Id = 3, Name = "John Doe", Gender = "Male", DOB = DateTime.Now.AddYears(-20) },
                new Patient { Id = 4, Name = "Jane Doe", Gender = "Female", DOB = DateTime.Now.AddYears(-25) },
                new Patient { Id = 5, Name = "John Doe", Gender = "Male", DOB = DateTime.Now.AddYears(-50) },
                new Patient { Id = 6, Name = "Jane Doe", Gender = "Female", DOB = DateTime.Now.AddYears(-55) },
            });

                _dbContext.appointments.AddRange(new List<Appointment>
            {
                new Appointment { Id = 1, PatientId = 1, RecepId = 1, DoctorId = 1, DateTime = DateTime.Now, CreatedAt = DateTime.Now, Status = "Completed" },
                new Appointment { Id = 2, PatientId = 2, DateTime = DateTime.Now },
                new Appointment { Id = 3, PatientId = 3, DateTime = DateTime.Now },
                new Appointment { Id = 4, PatientId = 4, DateTime = DateTime.Now },
                new Appointment { Id = 5, PatientId = 5, DateTime = DateTime.Now },
                new Appointment { Id = 6, PatientId = 6, DateTime = DateTime.Now },
            });

                _dbContext.prescriptions.AddRange(new List<Prescription>
            {
                new Prescription { Id = 1, CashierId = 1, AppointmentID = 1, DateTime = DateTime.Now, Total = 100.0f },
                new Prescription { Id = 2, AppointmentID = 2, DateTime = DateTime.Now, Total = 150.0f },
                new Prescription { Id = 3, AppointmentID = 3, DateTime = DateTime.Now, Total = 200.0f },
                new Prescription { Id = 4, AppointmentID = 4, DateTime = DateTime.Now, Total = 250.0f },
                new Prescription { Id = 5, AppointmentID = 5, DateTime = DateTime.Now, Total = 300.0f },
                new Prescription { Id = 6, AppointmentID = 6, DateTime = DateTime.Now, Total = 350.0f },
                new Prescription { Id = 7, DateTime = DateTime.Now.Date, Total = 100.0f }
            });

                _dbContext.users.Add(new User { Id = 1, Name = "Lab Assistant User" });
                _dbContext.labAssistants.Add(new LabAssistant { Id = 1, UserId = 1 });
                _dbContext.tests.AddRange(new List<Test>
            {
                new Test { Id = 1, TestName = "Blood Test", Abb = "BT", Provider = "Provider1" },
                new Test { Id = 2, TestName = "X-Ray", Abb = "XR", Provider = "Provider2" }
            });

                _dbContext.labReports.AddRange(new List<LabReport>
            {
                new LabReport { Id = 1, PrescriptionID = 7, TestId = 1, DateTime = DateTime.Now.Date ,Status = "" },
                new LabReport { Id = 2, PrescriptionID = 7, TestId = 1, DateTime = DateTime.Now.Date ,Status="" },
                new LabReport { Id = 3, PrescriptionID = 7, TestId = 2, DateTime = DateTime.Now.Date ,Status="" }
            });

                // Add drugs to the database
                _dbContext.drugs.AddRange(new List<Drug>
            {
                new Drug { Id = 1, BrandN = "DrugA", Weight = 500, Avaliable = 10,GenericN="A"},
                new Drug { Id = 2, BrandN = "DrugB", Weight = 250, Avaliable = 5,GenericN = "B"},
                new Drug { Id = 3, BrandN = "DrugC", Weight = 100, Avaliable = 20,GenericN="C"}
            });

                _dbContext.SaveChanges();
            }

            [Fact]
            public async Task GetMaleFemalePatientsCountAllDays_ShouldReturnCorrectCounts()
            {
                // Act
                var result = await _service.GetMaleFemalePatientsCountAllDays();

                // Assert
                Assert.NotNull(result);
                Assert.IsType<List<object>>(result);

                var countsByDay = result as List<object>;
                Assert.Single(countsByDay);

                var dayCounts = countsByDay.First();

                Assert.Equal(1, (int)dayCounts.GetType().GetProperty("child_male").GetValue(dayCounts, null));
                Assert.Equal(1, (int)dayCounts.GetType().GetProperty("child_female").GetValue(dayCounts, null));
                Assert.Equal(1, (int)dayCounts.GetType().GetProperty("adult_male").GetValue(dayCounts, null));
                Assert.Equal(1, (int)dayCounts.GetType().GetProperty("adult_female").GetValue(dayCounts, null));
                Assert.Equal(1, (int)dayCounts.GetType().GetProperty("old_male").GetValue(dayCounts, null));
                Assert.Equal(1, (int)dayCounts.GetType().GetProperty("old_female").GetValue(dayCounts, null));
            }

            [Fact]
            public async Task GetTotalAmount_ShouldReturnCorrectAmounts()
            {
                // Act
                var result = await _service.GetTotalAmount();

                // Assert
                Assert.NotNull(result);
                Assert.IsType<List<object>>(result);

                var totalsByDay = result as List<object>;
                Assert.Single(totalsByDay);

                var day1 = totalsByDay.FirstOrDefault(x => (DateTime)x.GetType().GetProperty("datefor").GetValue(x) == DateTime.Now.Date);

                Assert.NotNull(day1);
                Assert.Equal(1450.0f, (float)day1.GetType().GetProperty("income").GetValue(day1));
            }

            [Fact]
            public async Task GetAvailableCount_ShouldReturnCorrectCounts()
            {
                // Act
                var result = await _service.GetAvailableCount();

                // Assert
                Assert.NotNull(result);
                
            }


        
    }
}
