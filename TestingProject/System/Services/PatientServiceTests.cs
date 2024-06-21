using Moq;
using Services.AdminServices;
using DataAccessLayer;
using Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace TestingProject.System.Services
{
    public class PatientServiceTests
    {
        private readonly Mock<IRepository<Patient>> _mockRepository;
        private readonly PatientService _patientService;

        public PatientServiceTests()
        {
            _mockRepository = new Mock<IRepository<Patient>>();
            _patientService = new PatientService(_mockRepository.Object);
        }

        [Fact]
        public async Task AddPatient_ShouldCallRepositoryAdd()
        {
            // Arrange
            var patient = new Patient();

            // Act
            await _patientService.AddPatient(patient);

            // Assert
            _mockRepository.Verify(repo => repo.Add(patient), Times.Once);
        }

        [Fact]
        public async Task DeletePatient_ShouldCallRepositoryDelete()
        {
            // Arrange
            int patientId = 1;

            // Act
            await _patientService.DeletePatient(patientId);

            // Assert
            _mockRepository.Verify(repo => repo.Delete(patientId), Times.Once);
        }

        [Fact]
        public async Task GetPatient_ShouldReturnPatient()
        {
            // Arrange
            int patientId = 1;
            var expectedPatient = new Patient();
            _mockRepository.Setup(repo => repo.Get(patientId)).ReturnsAsync(expectedPatient);

            // Act
            var patient = await _patientService.GetPatient(patientId);

            // Assert
            Assert.Equal(expectedPatient, patient);
        }

        [Fact]
        public async Task GetAllPatients_ShouldReturnAllPatients()
        {
            // Arrange
            var expectedPatients = new List<Patient> { new Patient(), new Patient() };
            _mockRepository.Setup(repo => repo.GetAll()).ReturnsAsync(expectedPatients);

            // Act
            var patients = await _patientService.GetAllPatients();

            // Assert
            Assert.Equal(expectedPatients, patients);
        }

        [Fact]
        public async Task UpdatePatient_ShouldCallRepositoryUpdate()
        {
            // Arrange
            var patient = new Patient();

            // Act
            await _patientService.UpdatePatient(patient);

            // Assert
            _mockRepository.Verify(repo => repo.Update(patient), Times.Once);
        }
    }


}
