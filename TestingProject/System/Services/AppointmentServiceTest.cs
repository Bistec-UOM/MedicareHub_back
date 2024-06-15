using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using Models;
using Moq;
using Services.AppointmentService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestingProject.MockData;

namespace TestingProject.System.Services
{
    public class AppointmentServiceTest
    {
        private readonly ApplicationDbContext _context;

        public AppointmentServiceTest() {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new ApplicationDbContext(options);
            _context.Database.EnsureCreated();


        }


        [Fact]
        public async Task getAll_ReturnsAllAppointments()
        {
            //Arrange
            var mockAppointmentRepository = new Mock<IRepository<Appointment>>();
            var mockPatientRepository = new Mock<IRepository<Patient>>();
            var mockDoctorRepository = new Mock<IRepository<User>>();
            var mockUnableDateRepository = new Mock<IRepository<Unable_Date>>();
            var mockNotificationRepository = new Mock<IRepository<Notification>>();

            _context.appointments.AddRange(AppointmentMockData.getAppointments());
            _context.SaveChanges();
            



            mockAppointmentRepository.Setup(repo => repo.GetAll()).ReturnsAsync(AppointmentMockData.getAppointments());
            var sut = new AppointmentService(_context, mockAppointmentRepository.Object, mockPatientRepository.Object, mockDoctorRepository.Object, mockUnableDateRepository.Object, mockNotificationRepository.Object);


            //Act

            var result=await sut.GetAll();

            //Assert
            Assert.Equal(AppointmentMockData.getAppointments().Count, result.Count);
            Assert.Equal(AppointmentMockData.getAppointments()[0].Id, result[0].Id);
            Assert.Equal(AppointmentMockData.getAppointments()[1].Id, result[1].Id);

        }

        public void dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
