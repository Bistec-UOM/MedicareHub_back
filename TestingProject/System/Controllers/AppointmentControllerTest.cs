using API.Controllers;
using AppointmentNotificationHandler;
using DataAccessLayer;
using Microsoft.AspNetCore.SignalR;
using Moq;
using Services.AppointmentService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestingProject.MockData;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Models;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace TestingProject.System.Controllers
{
    public class AppointmentControllerTest:IDisposable
    {

         ApplicationDbContext _dbContext;
         IHubContext<AppointmentNotificationHub, IAppointmentNotificationClient> _hubContext;
        public AppointmentControllerTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(databaseName: "TestDatabase")
               .Options;

            _dbContext = new ApplicationDbContext(options);
            _dbContext.Database.EnsureCreated();
        }

        //----getAllAppointment()---
        [Fact]
        public async Task GetAllAppointments_sholudReturn200Status()
        {
            //arrange

            var appointmentService=new Mock<IAppointmentService>();
            appointmentService.Setup(_ => _.GetAll()).ReturnsAsync(AppointmentMockData.getAppointments());


            var sut = new AppointmentController(_dbContext, appointmentService.Object, _hubContext);

            //act

            var result=await sut.GetAllAppointments();


            //assert

            result.GetType().Should().Be(typeof(OkObjectResult));
            (result as OkObjectResult).StatusCode.Should().Be(200);
        }
        //-----GetDoctors()----
        [Fact]
        public async Task GetDoctors_ShouldCallGetDoctors()
        {
            //Arranage
            var appointmentService = new Mock<IAppointmentService>();
            appointmentService.Setup(_ => _.GetDoctors()).ReturnsAsync(AppointmentMockData.getDoctors());
            var sut=new AppointmentController(_dbContext,appointmentService.Object, _hubContext);


            //Act

            var result = await sut.GetDoctors();

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            okResult.StatusCode.Should().Be(200);
            appointmentService.Verify(_ => _.GetDoctors(),Times.Exactly(1));


        }
        //------AddAppointment()------
        [Fact]
        public async Task AddAppointment_ShouldCallServiceAddAppointment()
        {
            // Arrange
            var appointmentService = new Mock<IAppointmentService>();
            
            _dbContext.doctors.AddRange(AppointmentMockData.getDoctUserDoctors());

            // Set up a mock HttpContext with a claim
            var userClaims = new List<Claim>
    {
        new Claim("RoleId", "1") // Assuming RoleId is 1 for this test
    };

            var identity = new ClaimsIdentity(userClaims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            var httpContext = new DefaultHttpContext
            {
                User = claimsPrincipal
            };

            var controllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            var sut = new AppointmentController(_dbContext, appointmentService.Object, _hubContext)
            {
                ControllerContext = controllerContext
            };

            var appointmentToAdd = AppointmentMockData.AddNewAppointment();

            appointmentService.Setup(service => service.AddAppointment(It.IsAny<Appointment>())).ReturnsAsync(0);
                              

            // Act
            var result = await sut.AddAppointment(appointmentToAdd);

            // Assert

           

            var okResult = Assert.IsType<OkObjectResult>(result);  //for checking the 200 status code
            var resultValue = Assert.IsType<Int32>(okResult.Value); //for getting the value of okresult
            resultValue.Should().Be(0);
            appointmentService.Verify(service=>service.AddAppointment(It.IsAny<Appointment>()), Times.Once);
        }

        //--------getPatient(int id)----------
        [Fact]
        public async Task GetPatient_ShouldCallServiceGetPatient()
        {
            //Arranage
            var appointmentService = new Mock<IAppointmentService>();
            appointmentService.Setup(_ => _.GetPatient(AppointmentMockData.GetPatient().Id)).ReturnsAsync(AppointmentMockData.GetPatient());
            var sut = new AppointmentController(_dbContext, appointmentService.Object, _hubContext);

            //Act
            var result = await sut.GetPatient(AppointmentMockData.GetPatient().Id);

            //Assert

            var actionResult = Assert.IsType<ActionResult<Patient>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var resultValue = Assert.IsType<Patient>(okResult.Value);
            resultValue.Should().BeEquivalentTo(AppointmentMockData.GetPatient());
            Assert.Equal(AppointmentMockData.GetPatient().Id, resultValue.Id);
        }

        [Fact]
        public async Task GetPatient_ThrowsExcpetion()
        {
            //Arranage
            var appointmentService = new Mock<IAppointmentService>();
            appointmentService.Setup(_ => _.GetPatient(AppointmentMockData.GetPatient().Id)).Throws(new Exception("Test exception"));
            var sut = new AppointmentController(_dbContext, appointmentService.Object, _hubContext);

            //Act
            var result = await sut.GetPatient(AppointmentMockData.GetPatient().Id);

            //Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Test exception", badRequestResult.Value);


        }
        [Fact]
        public async Task GetPatient_InvaliId_ReturnsNotFound()
        {
            //Arranage
            var appointmentService = new Mock<IAppointmentService>();
            var patient = AppointmentMockData.GetPatient();
            appointmentService.Setup(service => service.GetPatient(It.IsAny<int>())).ReturnsAsync((Patient)null);
            var sut = new AppointmentController(_dbContext, appointmentService.Object, _hubContext);

            //Act
            var result = await sut.GetPatient(99);

            //Assert
            var actionResult = Assert.IsType<ActionResult<Patient>>(result);
            Assert.IsType<NotFoundResult>(actionResult.Result);

        }
        //-----------getAppointment(int id)-----
        [Fact]
        public async Task GetAppointment_ShouldCallServiceGetAppointment()
        {
            //Arranage
            var appointmentService = new Mock<IAppointmentService>();
            appointmentService.Setup(_ => _.GetAppointment(AppointmentMockData.getAppointment().Id)).ReturnsAsync(AppointmentMockData.getAppointment());
            var sut = new AppointmentController(_dbContext, appointmentService.Object, _hubContext);

            //Act
            var result = await sut.GetAppointment(AppointmentMockData.getAppointment().Id);

            //Assert

            var actionResult = Assert.IsType<ActionResult<Appointment>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var resultValue = Assert.IsType<Appointment>(okResult.Value);
            resultValue.Should().BeEquivalentTo(AppointmentMockData.getAppointment());
            Assert.Equal(AppointmentMockData.getAppointment().Id, resultValue.Id);

        }
        [Fact]
        public async Task GetAppointment_ThrowsExcpetion()
        {
            //Arranage
            var appointmentService = new Mock<IAppointmentService>();
            appointmentService.Setup(_ => _.GetAppointment(AppointmentMockData.getAppointment().Id)).Throws(new Exception("Test exception"));
            var sut = new AppointmentController(_dbContext, appointmentService.Object, _hubContext);

            //Act
            var result = await sut.GetAppointment(AppointmentMockData.getAppointment().Id);

            //Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Test exception", badRequestResult.Value);


        }

        [Fact]
        public async Task GetAppointment_InvaliId_ReturnsNotFound()
        {
            //Arranage
            var appointmentService = new Mock<IAppointmentService>();
            appointmentService.Setup(service => service.GetAppointment(It.IsAny<int>())).ReturnsAsync((Appointment)null);
            var sut = new AppointmentController(_dbContext, appointmentService.Object, _hubContext);

            //Act
            var result = await sut.GetAppointment(99);

            //Assert
            var actionResult = Assert.IsType<ActionResult<Appointment>>(result);
            Assert.IsType<NotFoundResult>(actionResult.Result);

        }

        //---getDoctorAppointments(int id)-----
        [Fact]
        public async Task getDoctorAppointment_shouldCallGetDoctorAppointments()
        {
            //Arranage
            var appointmentService = new Mock<IAppointmentService>();
            appointmentService.Setup(_ => _.GetDoctorAppointments(1)).ReturnsAsync(AppointmentMockData.getDoctor1Appointment());
            var sut = new AppointmentController(_dbContext, appointmentService.Object, _hubContext);


            //Act

            var result = await sut.GetDoctorAppointments(1);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            okResult.StatusCode.Should().Be(200);
            appointmentService.Verify(_ => _.GetDoctorAppointments(1), Times.Exactly(1));

        }
        [Fact]
        public async Task getDoctorAppointment_ThrowsException()
        {
            //Arranage
            var appointmentService = new Mock<IAppointmentService>();
            appointmentService.Setup(_ => _.GetDoctorAppointments(1)).Throws(new Exception("Test exception"));
            var sut = new AppointmentController(_dbContext, appointmentService.Object, _hubContext);


            //Act

            var result = await sut.GetDoctorAppointments(1);

            //Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Test exception", badRequestResult.Value);

        }

        //---GetPatients()---
        [Fact]
        public async Task GetPatients_ShouldCallGetPatients()
        {
            //Arranage
            var appointmentService = new Mock<IAppointmentService>();
            appointmentService.Setup(_ => _.GetPatients()).ReturnsAsync(AppointmentMockData.GetPatients());
            var sut = new AppointmentController(_dbContext, appointmentService.Object, _hubContext);


            //Act

            var result = await sut.GetPatients();

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            okResult.StatusCode.Should().Be(200);
            appointmentService.Verify(_ => _.GetPatients(), Times.Exactly(1));


        }
        [Fact]
        public async Task getPatients_ThrowsException()
        {
            //Arranage
            var appointmentService = new Mock<IAppointmentService>();
            appointmentService.Setup(_ => _.GetPatients()).Throws(new Exception("Test exception"));
            var sut = new AppointmentController(_dbContext, appointmentService.Object, _hubContext);


            //Act

            var result = await sut.GetPatients();

            //Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Test exception", badRequestResult.Value);

        }
        //---------RegisterPatient(patient patient)---
        [Fact]
        public async Task RegisterPatient_ShouldCallServiceRegisterPatient()
        {
            //Arranage
            var appointmentService = new Mock<IAppointmentService>();
            appointmentService.Setup(_ => _.RegisterPatient(AppointmentMockData.GetPatient())).Returns(Task.CompletedTask);

            var sut = new AppointmentController(_dbContext, appointmentService.Object, _hubContext);


            //Act

            var result = await sut.RegisterPatient(AppointmentMockData.GetPatient());

            //Assert
            var actionResult = Assert.IsType<OkResult>(result);
            appointmentService.Verify(service => service.RegisterPatient(It.IsAny<Patient>()), Times.Once);


        }
        [Fact]
        public async Task RegisterPatient_ThrowsException()
        {
            // Arrange
            var appointmentService = new Mock<IAppointmentService>();
            appointmentService.Setup(service => service.RegisterPatient(It.IsAny<Patient>())).ThrowsAsync(new Exception("Test exception"));

            var sut = new AppointmentController(_dbContext, appointmentService.Object, _hubContext);

            // Act
            var result = await sut.RegisterPatient(AppointmentMockData.GetPatient());

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Test exception", badRequestResult.Value);
        }
        //------- GetDoctorMonthAppointments(int doctorId, int mId)----
        [Fact]
        public async Task GetDoctorMonthAppointments_sholudCallServiceGetMonthAppointments()
        {
            //Arranage
            var appointmentService = new Mock<IAppointmentService>();
            appointmentService.Setup(_ => _.GetAppointmentCountOfDays(1,5)).ReturnsAsync(AppointmentMockData.getONlyDoctor1TuesDayAppointment());
            var sut = new AppointmentController(_dbContext, appointmentService.Object, _hubContext);


            //Act

            var result = await sut.GetDoctorMonthAppointments(1,5);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            okResult.StatusCode.Should().Be(200);
            appointmentService.Verify(_ => _.GetAppointmentCountOfDays(1,5), Times.Exactly(1));

        }

        [Fact]
        public async Task GetDoctorMonthAppointment_ThrowsAnException()
        {
            //Arranage
            var appointmentService = new Mock<IAppointmentService>();
            appointmentService.Setup(_ => _.GetAppointmentCountOfDays(1, 5)).Throws(new Exception("Test exception"));
            var sut = new AppointmentController(_dbContext, appointmentService.Object, _hubContext);


            //Act

            var result = await sut.GetDoctorMonthAppointments(1, 5);

            //Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Test exception", badRequestResult.Value);

        }
        //---------AddUnableDate(Unable_Date uDate)-----
        [Fact]
        public async Task AddUnableDate_ShouldCallServiceAddUnableDate()
        {
            //Arranage
            var appointmentService = new Mock<IAppointmentService>();
            appointmentService.Setup(_ => _.AddUnableDate(AppointmentMockData.AddNewUnableDate())).Returns(Task.CompletedTask);

            var sut = new AppointmentController(_dbContext, appointmentService.Object, _hubContext);


            //Act

            var result = await sut.AddUnableDate(AppointmentMockData.AddNewUnableDate());

            //Assert
            var actionResult = Assert.IsType<OkResult>(result);
            appointmentService.Verify(service => service.AddUnableDate(It.IsAny<Unable_Date>()), Times.Once);

        }
        [Fact]
        public async Task AddUnableDate_ThrowsException()
        {
            // Arrange
            var appointmentService = new Mock<IAppointmentService>();
            appointmentService.Setup(service => service.AddUnableDate(It.IsAny<Unable_Date>())).ThrowsAsync(new Exception("Test exception"));

            var sut = new AppointmentController(_dbContext, appointmentService.Object, _hubContext);

            // Act
            var result = await sut.AddUnableDate(AppointmentMockData.AddNewUnableDate());

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Test exception", badRequestResult.Value);
        }

        //--------GetUnableDates(int doctorId)----
        [Fact]
        public async Task GetUnableDates_sholudCallServicegetUnableDates()
        {
            //Arranage
            var appointmentService = new Mock<IAppointmentService>();
            appointmentService.Setup(_ => _.getUnableDates(1)).ReturnsAsync(AppointmentMockData.GetUnableDatesDoc1());
            var sut = new AppointmentController(_dbContext, appointmentService.Object, _hubContext);


            //Act

            var result = await sut.GetUnableDates(1);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            okResult.StatusCode.Should().Be(200);
            appointmentService.Verify(_ => _.getUnableDates(1), Times.Exactly(1));

        }
        [Fact]
        public async Task GetUnableDates_ThrowsAnException()
        {
            //Arranage
            var appointmentService = new Mock<IAppointmentService>();
            appointmentService.Setup(_ => _.getUnableDates(1)).Throws(new Exception("Test exception"));
            var sut = new AppointmentController(_dbContext, appointmentService.Object, _hubContext);


            //Act

            var result = await sut.GetUnableDates(1);

            //Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Test exception", badRequestResult.Value);

        }

        //-------AddNotification(Notification notification)---
        [Fact]
        public async Task AddNotification_shouldCallServiceAddNotification()
        {
            //Arranage
            var appointmentService = new Mock<IAppointmentService>();
            appointmentService.Setup(_ => _.AddNotification(AppointmentMockData.AddNewNotification())).Returns(Task.CompletedTask);

            var sut = new AppointmentController(_dbContext, appointmentService.Object, _hubContext);


            //Act

            var result = await sut.AddNotification(AppointmentMockData.AddNewNotification());

            //Assert
            var actionResult = Assert.IsType<OkResult>(result);
            appointmentService.Verify(service => service.AddNotification(It.IsAny<Notification>()), Times.Once);

        }
        [Fact]
        public async Task AddNotification_ThrowsException()
        {
            // Arrange
            var appointmentService = new Mock<IAppointmentService>();
            appointmentService.Setup(service => service.AddNotification(It.IsAny<Notification>())).ThrowsAsync(new Exception("Test exception"));

            var sut = new AppointmentController(_dbContext, appointmentService.Object, _hubContext);

            // Act
            var result = await sut.AddNotification(AppointmentMockData.AddNewNotification());

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Test exception", badRequestResult.Value);
        }

        //-------getNotifications(int userId)----
        [Fact]
        public async Task getNotifications_sholudCallServicegetNotifications()
        {
            //Arranage
            var appointmentService = new Mock<IAppointmentService>();
            appointmentService.Setup(_ => _.getNotifications(3)).ReturnsAsync(AppointmentMockData.GetNotificationsTo3());
            var sut = new AppointmentController(_dbContext, appointmentService.Object, _hubContext);


            //Act

            var result = await sut.getNotifications(3);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            okResult.StatusCode.Should().Be(200);
            appointmentService.Verify(_ => _.getNotifications(3), Times.Exactly(1));

        }

        [Fact]
        public async Task GetNotifications_ThrowsAnException()
        {
            //Arranage
            var appointmentService = new Mock<IAppointmentService>();
            appointmentService.Setup(_ => _.getNotifications(3)).Throws(new Exception("Test exception"));
            var sut = new AppointmentController(_dbContext, appointmentService.Object, _hubContext);


            //Act

            var result = await sut.getNotifications(3);

            //Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Test exception", badRequestResult.Value);

        }

        //----------getUnableTimeSlots(int doctorId,DateTime day)---
        [Fact]
        public async Task getUnableTimeSlots_sholudCallServicegetUnableTimeSlots()
        {
            //Arranage
            var appointmentService = new Mock<IAppointmentService>();
            appointmentService.Setup(_ => _.getUnableTimeslots(1, new DateTime(2024, 6, 18))).ReturnsAsync(AppointmentMockData.GetUnableTimeSlotsDoc1Date18);
            var sut = new AppointmentController(_dbContext, appointmentService.Object, _hubContext);


            //Act

            var result = await sut.getUnableTimeSlots(1, new DateTime(2024, 6, 18));

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            okResult.StatusCode.Should().Be(200);
            appointmentService.Verify(_ => _.getUnableTimeslots(1, new DateTime(2024, 6, 18)), Times.Exactly(1));

        }

        [Fact]

        public async Task getUnableTimeSlots_ThrowsAnException()
        {
            //Arranage
            var appointmentService = new Mock<IAppointmentService>();
            appointmentService.Setup(_ => _.getUnableTimeslots(1, new DateTime(2024, 6, 18))).Throws(new Exception("Test exception"));
            var sut = new AppointmentController(_dbContext, appointmentService.Object, _hubContext);


            //Act

            var result = await sut.getUnableTimeSlots(1, new DateTime(2024, 6, 18));

            //Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Test exception", badRequestResult.Value);

        }

        //---------UnblockDay(int id)----
        [Fact]
        public async Task UnblockDay_shouldCallServiceUnblockDay()
        {
            //Arranage
            var appointmentService = new Mock<IAppointmentService>();
            appointmentService.Setup(_ => _.UnblockDay(AppointmentMockData.GetUnableDatesDoc1()[0].Id)).ReturnsAsync(AppointmentMockData.GetUnableDatesDoc1()[0]);
            var sut = new AppointmentController(_dbContext, appointmentService.Object, _hubContext);


            //Act

            var result = await sut.UnblockDay(AppointmentMockData.GetUnableDatesDoc1()[0].Id);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            okResult.StatusCode.Should().Be(200);
            appointmentService.Verify(_ => _.UnblockDay(AppointmentMockData.GetUnableDatesDoc1()[0].Id), Times.Exactly(1));

        }
        [Fact]
        public async Task UnblockDay_ThrowsException()
        {
            //Arranage
            var appointmentService = new Mock<IAppointmentService>();
            appointmentService.Setup(_ => _.UnblockDay(AppointmentMockData.GetUnableDatesDoc1()[0].Id)).Throws(new Exception("Test exception"));
            var sut = new AppointmentController(_dbContext, appointmentService.Object, _hubContext);


            //Act

            var result = await sut.UnblockDay(AppointmentMockData.GetUnableDatesDoc1()[0].Id);

            //Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Test exception", badRequestResult.Value);


        }

        //---------markAsSeenNotifications(int userId, bool newSeenValue)---
        [Fact]
        public async Task MarkasSeenNotifications_ShouldCannServiceMarkasSeen()
        {
            //Arranage
            var appointmentService = new Mock<IAppointmentService>();
            appointmentService.Setup(service => service.markAsSeenNotifications(7, true))
             .Returns(Task.CompletedTask)
             .Verifiable();
            var sut = new AppointmentController(_dbContext, appointmentService.Object, _hubContext);


            //Act

            var result =  sut.MarkAsSeenNotifications(7,true);

            //Assert
           // result.Should().BeOfType<OkResult>();
            appointmentService.Verify(service => service.markAsSeenNotifications(7, true), Times.Once);


        }






        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

    }
}
