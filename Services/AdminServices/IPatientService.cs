using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace Services.AdminServices
{
    public interface IPatientService
    {
        Task AddPatient(Patient patient);
        Task DeletePatient(int id);
        Task<Patient> GetPatient(int id);
        Task<List<Patient>> GetAllPatients();
        Task UpdatePatient(Patient patient);
    }
}
