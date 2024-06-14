using DataAccessLayer;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.PharmacyService
{
    public class DrugsService
    {
        private readonly IRepository<Drug> _drg;
        private readonly ApplicationDbContext _cntx;
        public DrugsService(IRepository<Drug> drg, ApplicationDbContext cntx)
        {
            _drg = drg;
            _cntx = cntx;
        }

        public async Task<bool> AddDrug(Drug item)
        {
            // Check if the drug with the same combination of genericName, brandName, and weight already exists
            var existingDrug = _cntx.drugs
                .FirstOrDefault(d => d.GenericN == item.GenericN && d.BrandN == item.BrandN && d.Weight == item.Weight);

            if (existingDrug != null)
            {
                return false; // Drug already exists
            }

            await _drg.Add(item);
            return true; // Drug added successfully
        }
        public async Task<IEnumerable<Drug>> GetDrugs()
        {
            return await _drg.GetAll();
        }
        public async Task<bool> DeleteDrug(int id)
        {
            var existingDrug = await _drg.Get(id);
            if (existingDrug == null)
                return false;

            await _drg.Delete(id);
            return true;
        }
        public async Task<Drug> GetDrugById(int id)
        {
            return await _drg.Get(id);
        }
        public async Task<bool> UpdateDrug(int id, Drug updatedDrug)
        {
            var existingDrug = await _drg.Get(id);
            if (existingDrug == null)
                return false;

            existingDrug.BrandN = updatedDrug.BrandN;
            existingDrug.GenericN = updatedDrug.GenericN;
            existingDrug.Price = updatedDrug.Price;
            existingDrug.Weight = updatedDrug.Weight;
            existingDrug.Avaliable = updatedDrug.Avaliable;

            // Update other properties as needed

            await _drg.Update(existingDrug);
            return true;
        }
        public ServiceCharge GetServiceCharge()
        {
            return _cntx.serviceCharges.FirstOrDefault();
        }

        public bool UpdateServiceCharge(ServiceCharge updatedServiceCharge)
        {
            var existingServiceCharge = _cntx.serviceCharges.FirstOrDefault();
            if (existingServiceCharge == null)
                return false;

            // Assuming the ServiceCharge table has an Amount property
            existingServiceCharge.Price = updatedServiceCharge.Price;

            _cntx.serviceCharges.Update(existingServiceCharge);
            _cntx.SaveChanges();
            return true;
        }

    }
}
