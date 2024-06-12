using Models;
using Models.DTO.Lab.UploadResults;
using Models.DTO.Lab.ViewResults;
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

        public Task<Boolean> UplaodResults(Result data,int RoleId);

        public Task<List<VResult>> CheckResult(int Pid);

        public Task MarkCheck(List<int> ids);
    }
}
