using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.LabService
{
    public interface IValueServices
    {
        public Task<IEnumerable<Object>> RequestList();

        public Task<Boolean> AcceptSample(int id);

        public Task<IEnumerable<Object>> AcceptedSamplesList();

        public Task UplaodResults(List<Record> data);
    }  
}
