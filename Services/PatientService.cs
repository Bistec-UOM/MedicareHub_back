using DataAccessLayer;
using Models;


namespace Services
{
    public class PatientService : IPatientService
    {
        private readonly IRepository<Patient> _repository;
        public PatientService(IRepository<Patient> repository)
        {
            _repository = repository;
        }
        public void AddPatient(Patient patient)
        {
            _repository.Add(patient);
        }

        public void DeletePatient(int id)
        {
            _repository.Delete(id);
        }

        public Patient GetPatient(int id)
        {
            return _repository.Get(id);
        }

        public List<Patient> GetAllPatients()
        {
            return _repository.GetAll();
        }

        public void UpdatePatient(Patient patient)
        {
            _repository.Update(patient);
        }
    }
}
