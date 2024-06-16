using Castle.Core.Smtp;
using DataAccessLayer;
using FluentAssertions;
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
using IEmailSender = Services.AppointmentService.IEmailSender;

namespace TestingProject.System.Services
{
    public class AppointmentServiceTest:IDisposable
    {
        private readonly ApplicationDbContext _dbContext;

        public AppointmentServiceTest() {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _dbContext = new ApplicationDbContext(options);
            _dbContext.Database.EnsureCreated();


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

            _dbContext.appointments.AddRange(AppointmentMockData.getAppointments());
            _dbContext.SaveChanges();
            



            mockAppointmentRepository.Setup(repo => repo.GetAll()).ReturnsAsync(AppointmentMockData.getAppointments());
            var sut = new AppointmentService(_dbContext, mockAppointmentRepository.Object, mockPatientRepository.Object, mockDoctorRepository.Object, mockUnableDateRepository.Object, mockNotificationRepository.Object);


            //Act

            var result=await sut.GetAll();

            //Assert
            Assert.Equal(AppointmentMockData.getAppointments().Count, result.Count);
            Assert.Equal(AppointmentMockData.getAppointments()[0].Id, result[0].Id);
            Assert.Equal(AppointmentMockData.getAppointments()[1].Id, result[1].Id);

        }

        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [Fact]
        public async Task getDoctors_ReturnsAllDoctors()
        {
            //Arrange
            var mockAppointmentRepository = new Mock<IRepository<Appointment>>();
            var mockPatientRepository = new Mock<IRepository<Patient>>();
            var mockDoctorRepository = new Mock<IRepository<User>>();
            var mockUnableDateRepository = new Mock<IRepository<Unable_Date>>();
            var mockNotificationRepository = new Mock<IRepository<Notification>>();

            _dbContext.users.AddRange(AppointmentMockData.getUsers());
            _dbContext.SaveChangesAsync();

            var sut=new AppointmentService(_dbContext,mockAppointmentRepository.Object,mockPatientRepository.Object, mockDoctorRepository.Object,mockUnableDateRepository.Object,mockNotificationRepository.Object);

            //Act

            var result=await sut.GetDoctors();


            //Assert

            Assert.Equal(AppointmentMockData.getUsers().Count-1,result.Count);
            Assert.Equal(AppointmentMockData.getUsers()[0].Id, result[0].Id);
            Assert.Equal(AppointmentMockData.getUsers()[1].Id, result[1].Id);


        }
        [Fact]
        public async Task AddAppointment_ShouldAddAppointment()
        {

            //Arrange
            var mockAppointmentRepository = new Mock<IRepository<Appointment>>();
            var mockPatientRepository = new Mock<IRepository<Patient>>();
            var mockDoctorRepository = new Mock<IRepository<User>>();
            var mockUnableDateRepository = new Mock<IRepository<Unable_Date>>();
            var mockNotificationRepository = new Mock<IRepository<Notification>>();

           

            
            _dbContext.users.AddRange(AppointmentMockData.getUsers());
            _dbContext.doctors.AddRange(AppointmentMockData.getDoctUserDoctors());
            _dbContext.patients.AddRange(AppointmentMockData.GetPatients());
            _dbContext.appointments.AddRange(AppointmentMockData.getAppointments());
            _dbContext.SaveChangesAsync();

            var appointmentToAdd = AppointmentMockData.AddNewAppointment();

            mockAppointmentRepository.Setup(repo => repo.Add(It.IsAny<Appointment>()))
                   .Returns((Appointment appointment) =>
                   {
                       _dbContext.appointments.Add(appointment);
                       return Task.CompletedTask;
                   });



            var mockEmailSender = new Mock<IEmailSender>();
            mockEmailSender.Setup(sender => sender.SendMail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                           .Returns(Task.CompletedTask);
            var sut = new AppointmentService(_dbContext, mockAppointmentRepository.Object, mockPatientRepository.Object, mockDoctorRepository.Object, mockUnableDateRepository.Object, mockNotificationRepository.Object);
            
            //Act

            var result = await sut.AddAppointment(AppointmentMockData.AddNewAppointment());
            _dbContext.SaveChangesAsync();


            //Assert
            result.Should().Be(0);
            mockAppointmentRepository.Verify(repo=>repo.Add(It.IsAny<Appointment>()),Times.Once);
            _dbContext.appointments.Count().Should().Be(5);
           

        }

    }
}
