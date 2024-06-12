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
        public DrugsService(IRepository<Drug> drg)
        {
            _drg = drg;
        }

        public async Task AddDrug(Drug item)
        {
            await _drg.Add(item);
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

    }
}
