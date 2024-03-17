using DataAccessLayer;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.LabService
{
    public class ValueService
    {
        private readonly IRepository<Test> _tst;
        private readonly IRepository<LabReport> _rep;

        public ValueService(IRepository<Test> tst,IRepository<LabReport> rep)
        {
            _tst = tst;
            _rep = rep;
        }

        async public Task AcceptSample(int id)
        {
            var tmp= await _rep.Get(id);
            tmp.Status = "accepted";
            await _rep.Update(tmp);
        }


        async public Task<IEnumerable<LabReport>> AcceptedSamplesList()
        {
            return await _rep.GetByProp("Status", "accepted");
        }
    }
}
