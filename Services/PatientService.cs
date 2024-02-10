using DataAccessLayer;
using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services
{
    public class PatientService : IPatientService
    {
        private readonly IRepository<Patient> _repository;

        public PatientService(IRepository<Patient> repository)
        {
            _repository = repository;
        }

        public async Task AddPatientAsync(Patient patient)
        {
           await _repository.AddAsync(patient);
        }

        public void DeletePatient(int id)
        {
            _repository.DeleteAsync(id);
        }

        public async Task<Patient> GetPatientAsync(int id)
        {
            return await _repository.GetAsync(id);
        }

        public async Task<List<Patient>> GetAllPatientsAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task UpdatePatientAsync(Patient patient)
        {
            await _repository.UpdateAsync(patient);
        }
    }
}
