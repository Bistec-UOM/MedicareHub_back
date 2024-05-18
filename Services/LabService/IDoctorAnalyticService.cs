using Models.DTO.Lab.ViewResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.LabService
{
    public interface IDoctorAnalyticService
    {
        public Task<List<Object>> TrackDrugList(int id);
        public Task<List<VResult>> TrackReportList(int id);
    }
}
