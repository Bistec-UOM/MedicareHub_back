using Castle.Core.Smtp;
using DataAccessLayer;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Models;
using Moq;
using Services.AppointmentService;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _dbContext = new ApplicationDbContext(options);
            _dbContext.Database.EnsureCreatedAsync();

           

        }



      
        //----getAll()---
        [Fact]
        public async Task getAll_ReturnsAllAppointments()
        {
            ResetDatabase();

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

      
        //----getDoctors()---
        [Fact]
        public async Task getDoctors_ReturnsAllDoctors()
        {
            ResetDatabase();
            //Arrange
            var mockAppointmentRepository = new Mock<IRepository<Appointment>>();
            var mockPatientRepository = new Mock<IRepository<Patient>>();
            var mockDoctorRepository = new Mock<IRepository<User>>();
            var mockUnableDateRepository = new Mock<IRepository<Unable_Date>>();
            var mockNotificationRepository = new Mock<IRepository<Notification>>();

            _dbContext.users.AddRange(AppointmentMockData.getDoctors());
             await _dbContext.SaveChangesAsync();



            var sut=new AppointmentService(_dbContext,mockAppointmentRepository.Object,mockPatientRepository.Object, mockDoctorRepository.Object,mockUnableDateRepository.Object,mockNotificationRepository.Object);

            //Act
            
            var result =await  sut.GetDoctors();




            await _dbContext.SaveChangesAsync();
            //Assert
            Assert.Equal(AppointmentMockData.getDoctors().Count,result.Count);
            Assert.Equal(AppointmentMockData.getDoctors()[0].Id, result[0].Id);
            Assert.Equal(AppointmentMockData.getDoctors()[1].Id, result[1].Id);


        }
        //-----AddAppointment(Appointment app)---
        [Fact]
        public async Task AddAppointment_ShouldAddAppointment()
        {
            ResetDatabase();

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
            await _dbContext.SaveChangesAsync();

          //  var appointmentToAdd = AppointmentMockData.AddNewNotification();

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
           await _dbContext.SaveChangesAsync();


            //Assert
            result.Should().Be(0);
            mockAppointmentRepository.Verify(repo=>repo.Add(It.IsAny<Appointment>()),Times.Once);

            var appointmentsCount = await _dbContext.appointments.CountAsync();
            appointmentsCount.Should().Be(5);
            //_dbContext.appointments.Count().Should().Be(5);
           

        }
        //----GetPatient(int id)----
        [Fact]
        public async Task GetPatient_SholudReturnPatient()
        {
            ResetDatabase();
            //Arrange
            var mockAppointmentRepository = new Mock<IRepository<Appointment>>();
            var mockPatientRepository = new Mock<IRepository<Patient>>();
            var mockDoctorRepository = new Mock<IRepository<User>>();
            var mockUnableDateRepository = new Mock<IRepository<Unable_Date>>();
            var mockNotificationRepository = new Mock<IRepository<Notification>>();


            _dbContext.patients.AddRange(AppointmentMockData.GetPatients());
           await _dbContext.SaveChangesAsync();
            mockPatientRepository.Setup(repo => repo.Get(AppointmentMockData.GetPatient().Id)).ReturnsAsync(AppointmentMockData.GetPatient());
            var sut = new AppointmentService(_dbContext, mockAppointmentRepository.Object, mockPatientRepository.Object, mockDoctorRepository.Object, mockUnableDateRepository.Object, mockNotificationRepository.Object);


            //Act
            var result = await sut.GetPatient(AppointmentMockData.GetPatient().Id);


            //Assert

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(AppointmentMockData.GetPatient());
            mockPatientRepository.Verify(repo => repo.Get(AppointmentMockData.GetPatient().Id), Times.Once);


        }
        //----getAppointment(int id)----
        [Fact]
        public async Task GetAppointment_SholudReturnAppointment()
        {
            ResetDatabase();
            //Arrange
            var mockAppointmentRepository = new Mock<IRepository<Appointment>>();
            var mockPatientRepository = new Mock<IRepository<Patient>>();
            var mockDoctorRepository = new Mock<IRepository<User>>();
            var mockUnableDateRepository = new Mock<IRepository<Unable_Date>>();
            var mockNotificationRepository = new Mock<IRepository<Notification>>();

            _dbContext.appointments.AddRange(AppointmentMockData.getAppointments());
          await  _dbContext.SaveChangesAsync();
            mockAppointmentRepository.Setup(repo => repo.Get(AppointmentMockData.getAppointments()[0].Id)).ReturnsAsync(AppointmentMockData.getAppointments()[0]);
            var sut = new AppointmentService(_dbContext, mockAppointmentRepository.Object, mockPatientRepository.Object, mockDoctorRepository.Object, mockUnableDateRepository.Object, mockNotificationRepository.Object);

            //Act
            var result = await sut.GetAppointment(AppointmentMockData.getAppointments()[0].Id);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(AppointmentMockData.getAppointments()[0]);
            mockAppointmentRepository.Verify(repo => repo.Get(AppointmentMockData.getAppointments()[0].Id), Times.Once);


        }
        //----GetDoctorAppointments(int id)------
        [Fact]
        public async Task getDoctorAppointments_shouldReturnDoctorAppointment()
        {
            ResetDatabase();
            //Arrange
            var mockAppointmentRepository = new Mock<IRepository<Appointment>>();
            var mockPatientRepository = new Mock<IRepository<Patient>>();
            var mockDoctorRepository = new Mock<IRepository<User>>();
            var mockUnableDateRepository = new Mock<IRepository<Unable_Date>>();
            var mockNotificationRepository = new Mock<IRepository<Notification>>();

            _dbContext.appointments.AddRange(AppointmentMockData.getDoctor1Appointment());
          await  _dbContext.SaveChangesAsync();

            var sut = new AppointmentService(_dbContext, mockAppointmentRepository.Object, mockPatientRepository.Object, mockDoctorRepository.Object, mockUnableDateRepository.Object, mockNotificationRepository.Object);

            //Act

            var result = await sut.GetDoctorAppointments(1);


            //Assert

            Assert.Equal(AppointmentMockData.getDoctor1Appointment().Count, result.Count);
            Assert.Equal(AppointmentMockData.getDoctor1Appointment()[0].Id, result[0].Id);
            Assert.Equal(AppointmentMockData.getDoctor1Appointment()[1].Id, result[1].Id);


        }

        //---getPatients()---
        [Fact]
        public async Task getPatients_ReturnsAllPatients()
        {
            ResetDatabase();
            //Arrange
            var mockAppointmentRepository = new Mock<IRepository<Appointment>>();
            var mockPatientRepository = new Mock<IRepository<Patient>>();
            var mockDoctorRepository = new Mock<IRepository<User>>();
            var mockUnableDateRepository = new Mock<IRepository<Unable_Date>>();
            var mockNotificationRepository = new Mock<IRepository<Notification>>();

            _dbContext.patients.AddRange(AppointmentMockData.GetPatients());
          await  _dbContext.SaveChangesAsync();
            mockPatientRepository.Setup(repo => repo.GetAll()).ReturnsAsync(AppointmentMockData.GetPatients());

            var sut = new AppointmentService(_dbContext, mockAppointmentRepository.Object, mockPatientRepository.Object, mockDoctorRepository.Object, mockUnableDateRepository.Object, mockNotificationRepository.Object);

            //Act

            var result = await sut.GetPatients();


            //Assert

            Assert.Equal(AppointmentMockData.GetPatients().Count, result.Count);
            Assert.Equal(AppointmentMockData.GetPatients()[0].Id, result[0].Id);
            Assert.Equal(AppointmentMockData.GetPatients()[1].Id, result[1].Id);


        }

        //----RegisterPatient(Patient patient)
        [Fact]
        public async Task RegisterPatient_shouldCallRepoAdd()
        {
            ResetDatabase();
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
          await  _dbContext.SaveChangesAsync();

          ;

            mockPatientRepository.Setup(repo => repo.Add(It.IsAny<Patient>()))
                   .Returns((Patient patient) =>
                   {
                       _dbContext.patients.Add(patient);
                       
                       return Task.CompletedTask;
                   });

            await _dbContext.SaveChangesAsync();



            var mockEmailSender = new Mock<IEmailSender>();
            mockEmailSender.Setup(sender => sender.SendMail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                           .Returns(Task.CompletedTask);
            var sut = new AppointmentService(_dbContext, mockAppointmentRepository.Object, mockPatientRepository.Object, mockDoctorRepository.Object, mockUnableDateRepository.Object, mockNotificationRepository.Object);

            //Act

            var result =  sut.RegisterPatient(AppointmentMockData.GetPatient());
            await  _dbContext.SaveChangesAsync();


            //Assert
            mockPatientRepository.Verify(repo => repo.Add(It.IsAny<Patient>()), Times.Once);
            Assert.Equal(3,_dbContext.patients.Count()); ;

        }

        //------GetAppointmentCountOfDays(int doctorId,int monthId)

        [Fact]
        public async Task GetAppointmentCountOfDays_sholudReturnAppCount()
        {
            ResetDatabase();
            //Arrange
            var mockAppointmentRepository = new Mock<IRepository<Appointment>>();
            var mockPatientRepository = new Mock<IRepository<Patient>>();
            var mockDoctorRepository = new Mock<IRepository<User>>();
            var mockUnableDateRepository = new Mock<IRepository<Unable_Date>>();
            var mockNotificationRepository = new Mock<IRepository<Notification>>();

            _dbContext.users.AddRange(AppointmentMockData.getUsers());
            _dbContext.doctors.AddRange(AppointmentMockData.getDoctUserDoctors());
            _dbContext.patients.AddRange(AppointmentMockData.GetPatients());
            _dbContext.appointments.AddRange(AppointmentMockData.getONlyDoctor1TuesDayAppointment());
            await _dbContext.SaveChangesAsync();

            Debug.WriteLine("appcount",_dbContext.appointments.Count());

            var sut = new AppointmentService(_dbContext, mockAppointmentRepository.Object, mockPatientRepository.Object, mockDoctorRepository.Object, mockUnableDateRepository.Object, mockNotificationRepository.Object);

            //Act

            var result = await sut.GetAppointmentCountOfDays(1, 5);


            //Assert
            Debug.WriteLine("appcount", _dbContext.appointments.Count());
            Assert.Equal(AppointmentMockData.getONlyDoctor1TuesDayAppointment().Count, result.Count);
            Assert.Equal(AppointmentMockData.getONlyDoctor1TuesDayAppointment()[0].Id, result[0].Id);
            Assert.Equal(AppointmentMockData.getONlyDoctor1TuesDayAppointment()[1].Id, result[1].Id);

        }
        //-------AddUnableDate(Unable_Date uDate)----
        [Fact]
        public async Task AddUnableDates_ShouldCallRepoAdd()
        {
            ResetDatabase();
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
         await   _dbContext.SaveChangesAsync();

            ;
            _dbContext.unable_Dates.AddRange(AppointmentMockData.GetUnableDates());
            _dbContext.SaveChangesAsync();

            mockUnableDateRepository.Setup(repo => repo.Add(It.IsAny<Unable_Date>()))
                   .Returns((Unable_Date udate) =>
                   {
                       _dbContext.unable_Dates.Add(udate);
                       return Task.CompletedTask;
                   });



            var sut = new AppointmentService(_dbContext, mockAppointmentRepository.Object, mockPatientRepository.Object, mockDoctorRepository.Object, mockUnableDateRepository.Object, mockNotificationRepository.Object);

            //Act

            var result = sut.AddUnableDate(AppointmentMockData.AddNewUnableDate());
           await _dbContext.SaveChangesAsync();


            //Assert
            mockUnableDateRepository.Verify(repo => repo.Add(It.IsAny<Unable_Date>()), Times.Once);
            Assert.Equal(_dbContext.unable_Dates.Count(), 3); ;

        }

        //-----getUnableDates(int doctorId)----
        [Fact]
        public async Task GetUnableDates_sholudReturnUnableDates()
        {
            ResetDatabase();
            //Arrange
            var mockAppointmentRepository = new Mock<IRepository<Appointment>>();
            var mockPatientRepository = new Mock<IRepository<Patient>>();
            var mockDoctorRepository = new Mock<IRepository<User>>();
            var mockUnableDateRepository = new Mock<IRepository<Unable_Date>>();
            var mockNotificationRepository = new Mock<IRepository<Notification>>();

            _dbContext.unable_Dates.AddRange(AppointmentMockData.GetUnableDatesDoc1());
           await _dbContext.SaveChangesAsync();

            var sut = new AppointmentService(_dbContext, mockAppointmentRepository.Object, mockPatientRepository.Object, mockDoctorRepository.Object, mockUnableDateRepository.Object, mockNotificationRepository.Object);

            //Act

            var result = await sut.getUnableDates(1);


            //Assert

            Assert.Equal(AppointmentMockData.GetUnableDatesDoc1().Count, result.Count);
            Assert.Equal(AppointmentMockData.GetUnableDatesDoc1()[0].Id, result[0].Id);
            Assert.Equal(AppointmentMockData.GetUnableDatesDoc1()[1].Id, result[1].Id);

        }


        //-----getUnableTimeslots(int doctorId,DateTime day)---

        [Fact]
        public async Task GetUnableTimeSlots_sholudReturnUnableTimeSlots()
        {
            ResetDatabase();
            //Arrange
            var mockAppointmentRepository = new Mock<IRepository<Appointment>>();
            var mockPatientRepository = new Mock<IRepository<Patient>>();
            var mockDoctorRepository = new Mock<IRepository<User>>();
            var mockUnableDateRepository = new Mock<IRepository<Unable_Date>>();
            var mockNotificationRepository = new Mock<IRepository<Notification>>();

            _dbContext.unable_Dates.AddRange(AppointmentMockData.GetUnableTimeSlotsDoc1Date18());
            await _dbContext.SaveChangesAsync();

            var sut = new AppointmentService(_dbContext, mockAppointmentRepository.Object, mockPatientRepository.Object, mockDoctorRepository.Object, mockUnableDateRepository.Object, mockNotificationRepository.Object);

            //Act

            var result = await sut.getUnableTimeslots(1, new DateTime(2024, 6, 18));


            //Assert

            Assert.Equal(AppointmentMockData.GetUnableTimeSlotsDoc1Date18().Count, result.Count);
            Assert.Equal(AppointmentMockData.GetUnableTimeSlotsDoc1Date18()[0].Id, result[0].Id);
            Assert.Equal(AppointmentMockData.GetUnableTimeSlotsDoc1Date18()[1].Id, result[1].Id);

        }


        //------------ getDoctor(int doctorId)----
        [Fact]
        public async Task getDoctor_sholudReturnDoctor()
        {
            ResetDatabase();
            //Arrange
            var mockAppointmentRepository = new Mock<IRepository<Appointment>>();
            var mockPatientRepository = new Mock<IRepository<Patient>>();
            var mockDoctorRepository = new Mock<IRepository<User>>();
            var mockUnableDateRepository = new Mock<IRepository<Unable_Date>>();
            var mockNotificationRepository = new Mock<IRepository<Notification>>();

            _dbContext.doctors.AddRange(AppointmentMockData.getDoctUserDoctors());
            await _dbContext.SaveChangesAsync();

            var sut = new AppointmentService(_dbContext, mockAppointmentRepository.Object, mockPatientRepository.Object, mockDoctorRepository.Object, mockUnableDateRepository.Object, mockNotificationRepository.Object);

            //Act

            var result = await sut.getDoctor(1);


            //Assert
            result.Should().NotBeNull();
            Assert.Equal(AppointmentMockData.getDoctUserDoctors()[0].Id, result.Id);
            Assert.Equal(AppointmentMockData.getDoctUserDoctors()[0].UserId, result.UserId);

        }


        //----------getUser(int userId)---
        [Fact]
        public async Task getUser_shouldReturnsUser()
        {
            ResetDatabase();
            //Arrange
            var mockAppointmentRepository = new Mock<IRepository<Appointment>>();
            var mockPatientRepository = new Mock<IRepository<Patient>>();
            var mockUserRepository = new Mock<IRepository<User>>();
            var mockUnableDateRepository = new Mock<IRepository<Unable_Date>>();
            var mockNotificationRepository = new Mock<IRepository<Notification>>();

           
            await _dbContext.SaveChangesAsync();
            mockUserRepository.Setup(repo => repo.Get(AppointmentMockData.getUsers()[0].Id)).ReturnsAsync(AppointmentMockData.getUsers()[0]);

            var sut = new AppointmentService(_dbContext, mockAppointmentRepository.Object, mockPatientRepository.Object, mockUserRepository.Object, mockUnableDateRepository.Object, mockNotificationRepository.Object);

            //Act

            var result = await sut.getUser(AppointmentMockData.getUsers()[0].Id);


            //Assert

            mockUserRepository.Verify(repo => repo.Get(It.IsAny<int>()), Times.Once);
            Assert.Equal(AppointmentMockData.getUsers()[0].Id, result.Id);
            Assert.Equal(AppointmentMockData.getUsers()[0].Name, result.Name);


        }


        //---------- UnblockDay(int id)----------
        [Fact]
        public async Task UnblockDay_shouldDeleteBlockedDay()
        {
            ResetDatabase();
            //Arrange
            var mockAppointmentRepository = new Mock<IRepository<Appointment>>();
            var mockPatientRepository = new Mock<IRepository<Patient>>();
            var mockDoctorRepository = new Mock<IRepository<User>>();
            var mockUnableDateRepository = new Mock<IRepository<Unable_Date>>();
            var mockNotificationRepository = new Mock<IRepository<Notification>>();

            _dbContext.unable_Dates.AddRange(AppointmentMockData.GetUnableDatesDoc1());
            await _dbContext.SaveChangesAsync();

            mockUnableDateRepository.Setup(repo => repo.Delete(It.IsAny<int>()))
            .Returns<int>(async (id) =>
            {
                var day = await _dbContext.unable_Dates.FindAsync(id);
                _dbContext.unable_Dates.Remove(day);
                await _dbContext.SaveChangesAsync();
            });

            var sut = new AppointmentService(_dbContext, mockAppointmentRepository.Object, mockPatientRepository.Object, mockDoctorRepository.Object, mockUnableDateRepository.Object, mockNotificationRepository.Object);

            //Act

            var result = await sut.UnblockDay(1);


            //Assert

            Assert.Equal(AppointmentMockData.GetUnableDatesDoc1().Count-1, _dbContext.unable_Dates.Count());
            Assert.Equal(AppointmentMockData.GetUnableTimeSlotsDoc1Date18()[0].Id, result.Id);

        

        }


        //--------------markAsSeenNotifications(int userId, bool newSeenValue)---
        [Fact]
        public async Task MarkAsSeenNotification_shouldUpdateSeenValue()
        {
            ResetDatabase();
            //Arrange
            var mockAppointmentRepository = new Mock<IRepository<Appointment>>();
            var mockPatientRepository = new Mock<IRepository<Patient>>();
            var mockDoctorRepository = new Mock<IRepository<User>>();
            var mockUnableDateRepository = new Mock<IRepository<Unable_Date>>();
            var mockNotificationRepository = new Mock<IRepository<Notification>>();

            _dbContext.notification.AddRange(AppointmentMockData.GetNotificationsTo3());
            await _dbContext.SaveChangesAsync();

            var sut = new AppointmentService(_dbContext, mockAppointmentRepository.Object, mockPatientRepository.Object, mockDoctorRepository.Object, mockUnableDateRepository.Object, mockNotificationRepository.Object);

            //Act

            var result =  sut.markAsSeenNotifications(3,true);


            //Assert

            var updatedNotifications = _dbContext.notification.Where(n => n.To == 3.ToString()).ToList();
            updatedNotifications.Should().AllSatisfy(n => n.Seen.Should().BeTrue());


        }

        //-----------UpdateOnlyOneAppointmentUsingId(int id)---------

        [Fact]
        public async Task UpdateOnlyOneAppointmentUsingId_ShouldUpdateStatus()
        {
            ResetDatabase();
            //Arrange
            var mockAppointmentRepository = new Mock<IRepository<Appointment>>();
            var mockPatientRepository = new Mock<IRepository<Patient>>();
            var mockDoctorRepository = new Mock<IRepository<User>>();
            var mockUnableDateRepository = new Mock<IRepository<Unable_Date>>();
            var mockNotificationRepository = new Mock<IRepository<Notification>>();

            _dbContext.appointments.AddRange(AppointmentMockData.getAppointmentToBeUpdated());
            _dbContext.patients.AddRange(AppointmentMockData.getUpdatedAppointmentPatient());
            _dbContext.SaveChangesAsync();

            mockAppointmentRepository.Setup(repo => repo.Get(AppointmentMockData.getAppointmentToBeUpdated().Id)).ReturnsAsync(AppointmentMockData.getAppointmentToBeUpdated());

            mockPatientRepository.Setup(repo => repo.Get(AppointmentMockData.getUpdatedAppointmentPatient().Id)).ReturnsAsync(AppointmentMockData.getUpdatedAppointmentPatient());


            var sut = new AppointmentService(_dbContext, mockAppointmentRepository.Object, mockPatientRepository.Object, mockDoctorRepository.Object, mockUnableDateRepository.Object, mockNotificationRepository.Object);

            //Act

            var result =await  sut.UpdateOnlyOneAppointmentUsingId(AppointmentMockData.getAppointmentToBeUpdated().Id);
            //Asserrt

            Assert.NotNull(result);
            Assert.Equal("cancelled", result.Status);
            mockAppointmentRepository.Verify(repo => repo.Get(AppointmentMockData.getAppointmentToBeUpdated().Id), Times.Once);

        }


        //------------UpdateAppointmentStatus---------
        [Fact]
        public async Task UpdateAppointmentStatus_ShouldUpdateStatus()
        {
            ResetDatabase();
            //Arrange
            var mockAppointmentRepository = new Mock<IRepository<Appointment>>();
            var mockPatientRepository = new Mock<IRepository<Patient>>();
            var mockDoctorRepository = new Mock<IRepository<User>>();
            var mockUnableDateRepository = new Mock<IRepository<Unable_Date>>();
            var mockNotificationRepository = new Mock<IRepository<Notification>>();

            _dbContext.appointments.AddRange(AppointmentMockData.getAppointmentToBeUpdated());
            _dbContext.patients.AddRange(AppointmentMockData.getUpdatedAppointmentPatient());
            _dbContext.SaveChangesAsync();

            mockAppointmentRepository.Setup(repo => repo.Get(AppointmentMockData.getAppointmentToBeUpdated().Id)).ReturnsAsync(AppointmentMockData.getAppointmentToBeUpdated());

            mockPatientRepository.Setup(repo => repo.Get(AppointmentMockData.getUpdatedAppointmentPatient().Id)).ReturnsAsync(AppointmentMockData.getUpdatedAppointmentPatient());


            var sut = new AppointmentService(_dbContext, mockAppointmentRepository.Object, mockPatientRepository.Object, mockDoctorRepository.Object, mockUnableDateRepository.Object, mockNotificationRepository.Object);

            //Act

            var result = await sut.UpdateAppointmentStatus(AppointmentMockData.getAppointmentToBeUpdated().Id,AppointmentMockData.getnewUpdatedAppointment());
            //Asserrt

            Assert.NotNull(result);
            Assert.Equal("Cancelled", result.Status);
            mockAppointmentRepository.Verify(repo => repo.Get(AppointmentMockData.getAppointmentToBeUpdated().Id), Times.Once);

        }



        //------------UpdateAppointment---------
        [Fact]
        public async Task UpdateAppointment_ShouldUpdateAppointment()
        {
            ResetDatabase();
            //Arrange
            var mockAppointmentRepository = new Mock<IRepository<Appointment>>();
            var mockPatientRepository = new Mock<IRepository<Patient>>();
            var mockDoctorRepository = new Mock<IRepository<User>>();
            var mockUnableDateRepository = new Mock<IRepository<Unable_Date>>();
            var mockNotificationRepository = new Mock<IRepository<Notification>>();

            _dbContext.appointments.AddRange(AppointmentMockData.getAppointmentToBeUpdated());
            _dbContext.patients.AddRange(AppointmentMockData.getUpdatedAppointmentPatient());
            _dbContext.doctors.AddRange(AppointmentMockData.getUpDatedAppointmentDoctor());
            _dbContext.users.AddRange(AppointmentMockData.getUpdateAppointmentDoctorUser());
            _dbContext.SaveChangesAsync();

            mockAppointmentRepository.Setup(repo => repo.Get(AppointmentMockData.getAppointmentToBeUpdated().Id)).ReturnsAsync(AppointmentMockData.getAppointmentToBeUpdated());

            mockPatientRepository.Setup(repo => repo.Get(AppointmentMockData.getUpdatedAppointmentPatient().Id)).ReturnsAsync(AppointmentMockData.getUpdatedAppointmentPatient());


            var sut = new AppointmentService(_dbContext, mockAppointmentRepository.Object, mockPatientRepository.Object, mockDoctorRepository.Object, mockUnableDateRepository.Object, mockNotificationRepository.Object);

            //Act

            var result = await sut.UpdateAppointment(AppointmentMockData.getAppointmentToBeUpdated().Id, AppointmentMockData.getnewUpdatedAppointment());
            //Asserrt

            result.Should().Be(0);
            mockPatientRepository.Verify(repo => repo.Get(AppointmentMockData.getUpdatedAppointmentPatient().Id), Times.Once);

        }

        //--------------CancelAllAppointments(int doctorId, DateTime date)-------
        [Fact]
        public async Task CancelAllAppointments_OfaDayByDoctor()
        {

            ResetDatabase();
            //Arrange
            var mockAppointmentRepository = new Mock<IRepository<Appointment>>();
            var mockPatientRepository = new Mock<IRepository<Patient>>();
            var mockDoctorRepository = new Mock<IRepository<User>>();
            var mockUnableDateRepository = new Mock<IRepository<Unable_Date>>();
            var mockNotificationRepository = new Mock<IRepository<Notification>>();

            _dbContext.appointments.AddRange(AppointmentMockData.getAppointments_DoctorToBeCancelled());
            _dbContext.patients.AddRange(AppointmentMockData.getAppointmentCancelledPatients());
            _dbContext.SaveChangesAsync();

            mockAppointmentRepository.Setup(repo => repo.Get(It.IsAny<int>())).ReturnsAsync((int id) => _dbContext.appointments.FirstOrDefault(a => a.Id == id));
            mockPatientRepository.Setup(repo => repo.Get(It.IsAny<int>())).ReturnsAsync((int id) => _dbContext.patients.FirstOrDefault(a => a.Id == id));

            var sut = new AppointmentService(_dbContext, mockAppointmentRepository.Object, mockPatientRepository.Object, mockDoctorRepository.Object, mockUnableDateRepository.Object, mockNotificationRepository.Object);



            //Act


            var result = await sut.CancelAllAppointments(AppointmentMockData.getAppointments_DoctorToBeCancelled()[0].DoctorId, AppointmentMockData.getAppointments_DoctorToBeCancelled()[0].DateTime.Date);


            //Assert
            Assert.NotNull(result);
            Assert.Equal(_dbContext.appointments.Count(), result.Count);
            Assert.All(result, appointment => Assert.Equal("cancelled", appointment.Status));





        }


        //---------------DeleteAllDoctorDayAppointments(int doctorId, DateTime date)---------------
        [Fact]
        public async Task DeleteAllDoctorDayAppointments_ShouldDeleteAllApps()
        {

            ResetDatabase();
            //Arrange
            var mockAppointmentRepository = new Mock<IRepository<Appointment>>();
            var mockPatientRepository = new Mock<IRepository<Patient>>();
            var mockDoctorRepository = new Mock<IRepository<User>>();
            var mockUnableDateRepository = new Mock<IRepository<Unable_Date>>();
            var mockNotificationRepository = new Mock<IRepository<Notification>>();

            _dbContext.appointments.AddRange(AppointmentMockData.getDeletedAppointments());
            _dbContext.SaveChangesAsync();

           

            var sut = new AppointmentService(_dbContext, mockAppointmentRepository.Object, mockPatientRepository.Object, mockDoctorRepository.Object, mockUnableDateRepository.Object, mockNotificationRepository.Object);



            //Act


            var result = await sut.DeleteAllDoctorDayAppointments(AppointmentMockData.getDeletedAppointments()[0].DoctorId, AppointmentMockData.getDeletedAppointments()[0].DateTime.Date);


            //Assert
            Assert.NotNull(result);
            Assert.Equal(AppointmentMockData.getDeletedAppointments().Count, result.Count);
            Assert.Equal(_dbContext.appointments.Count(),0);





        }

        //---------DeleteAppointment(int id) ---------
        [Fact]
        public async Task DeleteAppointment_shouldDeleteApp()
        {

            ResetDatabase();
            //Arrange
            var mockAppointmentRepository = new Mock<IRepository<Appointment>>();
            var mockPatientRepository = new Mock<IRepository<Patient>>();
            var mockDoctorRepository = new Mock<IRepository<User>>();
            var mockUnableDateRepository = new Mock<IRepository<Unable_Date>>();
            var mockNotificationRepository = new Mock<IRepository<Notification>>();

            _dbContext.appointments.AddRange(AppointmentMockData.getDeletedAppointments());
            _dbContext.SaveChangesAsync();

            mockAppointmentRepository.Setup(repo => repo.Get(It.IsAny<int>())).ReturnsAsync((int id) => _dbContext.appointments.FirstOrDefault(a => a.Id == id));
            mockAppointmentRepository.Setup(repo => repo.Delete(It.IsAny<int>()))
      .Callback<int>(id =>
      {
          var appointment = _dbContext.appointments.FirstOrDefault(a => a.Id == id);
          if (appointment != null)
          {
              _dbContext.appointments.Remove(appointment);
              _dbContext.SaveChanges();
          }
      });
            var sut = new AppointmentService(_dbContext, mockAppointmentRepository.Object, mockPatientRepository.Object, mockDoctorRepository.Object, mockUnableDateRepository.Object, mockNotificationRepository.Object);



            //Act


            var result = await sut.DeleteAppointment(AppointmentMockData.getDeletedAppointments()[0].Id);


            //Assert
            Assert.NotNull(result);
            Assert.Equal(_dbContext.appointments.Count(), AppointmentMockData.getDeletedAppointments().Count-1);
            Assert.Equal(AppointmentMockData.getDeletedAppointments()[0].Id,result.Id);





        }
















        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        private void ResetDatabase()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Database.EnsureCreated();
        }










    }
}
