using DataAccessLayer;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class TemplateService
    {
        private readonly IDBop<ReportFileds> _tmplt;
        public TemplateService(IDBop<ReportFileds> tmplt) 
        { 
            _tmplt = tmplt;
        }

        public async Task<IEnumerable<ReportFileds>> GetAllFields()
        {
            return await _tmplt.GetAll();
        }

        public async Task<ReportFileds> GetField(int id)
        {
            return await _tmplt.Get(id);
        }

        public async Task AddField(ReportFileds item)
        {
            await _tmplt.Add(item);
        }

        public async Task EditField(ReportFileds item)
        {
            await _tmplt.Update(item);
        }

        public async Task DeleteField(int id)
        {
            var x = await _tmplt.Get(id);
            if (x != null)
            {
                await _tmplt.Delete(id);
            }
        }
    }
}
