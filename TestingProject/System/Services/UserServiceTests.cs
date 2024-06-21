using Xunit;
using Moq;
using Services.AdminServices;
using DataAccessLayer;
using Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace TestingProject.System.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IRepository<User>> _userRepositoryMock;
        private readonly ApplicationDbContext _dbContext;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _userRepositoryMock = new Mock<IRepository<User>>();

            // Use in-memory database
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _dbContext = new ApplicationDbContext(options);
            _userService = new UserService(_userRepositoryMock.Object, _dbContext);
        }

        [Fact]
        public async Task AddUser_Should_Add_User_To_Repository()
        {
            // Arrange
            var user = new User
            {
                // Set a valid password for the user
                Id = 1,
                Name = "Test",
                FullName = "Test",
                ContactNumber = "Test",
                Password = "ValidPassword123",
                Role = "Doctor", // or any valid role
                IsActive = true,
                IsDeleted = false,
                
            };

            // Act
            await _userService.AddUser(user);

            // Assert
            _userRepositoryMock.Verify(repo => repo.Add(user), Times.Once);
        }

        [Fact]
        public async Task DeletePatient_ShouldCallRepositoryDelete()
        {
            // Arrange
            int userId = 1;

            // Act
            await _userService.DeleteUser(userId);

            // Assert
            _userRepositoryMock.Verify(repo => repo.Delete(userId), Times.Once);
        }

        [Fact]
        public async Task GetPatient_ShouldReturnPatient()
        {
            // Arrange
            int userId = 1;
            var expectedUser = new User();
            _userRepositoryMock.Setup(repo => repo.Get(userId)).ReturnsAsync(expectedUser);

            // Act
            var userD = await _userService.GetUser(userId);

            // Assert
            Assert.Equal(expectedUser, userD);
        }

        [Fact]
        public async Task GetAllPatients_ShouldReturnAllPatients()
        {
            // Arrange
            var expectedPatients = new List<User> { new User(), new User() };
            _userRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(expectedPatients);

            // Act
            var patients = await _userService.GetAllUsers();

            // Assert
            Assert.Equal(expectedPatients, patients);
        }

        [Fact]
        public async Task UpdatePatient_ShouldCallRepositoryUpdate()
        {
            // Arrange
            var users = new User
            {
                // Set a valid password for the user
                Id = 1,
                Name = "Test",
                FullName = "Test",
                ContactNumber = "Test",
                Password = "ValidPassword123",
                Role = "Doctor", // or any valid role
                IsActive = true,
                IsDeleted = false,

            };

            // Act
            await _userService.UpdateUser(users);

            // Assert
            _userRepositoryMock.Verify(repo => repo.Update(users), Times.Once);
        }
    }
}
