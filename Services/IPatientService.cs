using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace Services
{
    public interface IPatientService
    {
        Task AddPatientAsync(Patient patient);
        void DeletePatient(int id);
        Task<Patient> GetPatientAsync(int id);
        Task<List<Patient>> GetAllPatientsAsync();
        public void UpdatePatient(Patient patient);
    }
}
