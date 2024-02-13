using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.DTO;
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
        private readonly IDBop<ReportFields> _tmplt;
        private readonly IDBop<Test> _tst;
        public TemplateService(IDBop<ReportFields> tmplt,IDBop<Test> tst) 
        { 
            _tmplt = tmplt;
            _tst = tst;
        }

        public async Task<IEnumerable<ReportFields>> GetAllFields()
        {
            return await _tmplt.GetAll();
        }

        public async Task<ReportFields> GetField(int id)
        {
            return await _tmplt.Get(id);
        }

        //public async Task AddField(List<ReportFields> item)
        //{
        //    foreach (var i in item)
        //    {
        //        await _tmplt.Add(i);
        //    }
        //}

        //public async Task AddTemplate(Test item,List<ReportFields> item2)
        //{
        //    await _tst.Add(item);
        //    foreach(var i in item2)
        //    {
        //        await _tmplt.Add(i);
        //    }
        //}

        public async Task AddTemplate(TemplateObj data)
        {
            var x = new Test 
            { 
                TestName= data.TestName,
                Price= data.Price,
                Provider= data.Provider,
            };
            await _tst.Add(x);

            var testId = x.TestId;

            foreach(var i in data.ReportFields) 
            {
                await _tmplt.Add(new ReportFields
                {
                    Fieldname= i.Fieldname,
                    Index=i.Index,
                    MinRef=i.MinRef,
                    MaxRef=i.MaxRef,
                    Unit=i.Unit,
                    TestId=testId
                });  
            }
        }

        public async Task EditField(ReportFields item)
        {
            await _tmplt.Update(item);
        }

        public async Task EditTemplate(List<ReportFields> data)
        {
            foreach (var i in data)
            {
                await _tmplt.Update(i);
            }
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
