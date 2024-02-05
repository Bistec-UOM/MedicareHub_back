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
        void AddPatient(Patient patient);
        Patient GetPatient(int id);
        List<Patient> GetAllPatients();
        void UpdatePatient(Patient patient);
        void DeletePatient(int id);
    }
}
