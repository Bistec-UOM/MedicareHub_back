using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Services
{
    public class TemplateService
    {
        private readonly IDBop<ReportFileds> _tmplt;
        private readonly IDBop<Test> _tst;
        public TemplateService(IDBop<ReportFileds> tmplt,IDBop<Test> tst) 
        { 
            _tmplt = tmplt;
            _tst = tst;
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

        public async Task AddTemplate(Test item,List<ReportFileds> item2)
        {
            await _tst.Add(item);
            foreach(var i in item2)
            {
                await _tmplt.Add(i);
            }
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
