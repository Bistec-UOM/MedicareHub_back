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
    public class LabController : IDisposable
    {
        ApplicationDbContext _dbContext;
        IHubContext<AppointmentNotificationHub, IAppointmentNotificationClient> _hubContext;
        public LabController()
        {
          var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

          _dbContext = new ApplicationDbContext(options);
          _dbContext.Database.EnsureCreated();
        }

        [Fact]
        public async Task GetAllTests()
        {


        }


        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }
    }
}
