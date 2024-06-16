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

        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

    }
}
