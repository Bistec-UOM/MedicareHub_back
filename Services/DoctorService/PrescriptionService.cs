using DataAccessLayer;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DoctorService
{
    public class PrescriptionService
    {
        private readonly IRepository<Prescription> _pres;
        public PrescriptionService(IRepository<Prescription> pres)
        {
            _pres = pres;
        }
        public async Task AddPrescription(Prescription item)
        {
            await _pres.Add(item);
        }
        public async Task<IEnumerable<Prescription>> GetPrescription()
        {
            return await _pres.GetAll();
        }
        public async Task<bool> DeletePrescription(int id)
        {
            var existingDrug = await _pres.Get(id);
            if (existingDrug == null)
                return false;

            await _pres.Delete(id);
            return true;
        }
    }
}
