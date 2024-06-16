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

namespace TestingProject.System.Controllers
{
    public class AppointmentControllerTest
    {

         ApplicationDbContext _dbContext;
         IHubContext<AppointmentNotificationHub, IAppointmentNotificationClient> _hubContext;


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

    }
}
