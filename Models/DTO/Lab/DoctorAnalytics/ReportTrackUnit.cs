using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO.Lab.DoctorAnalytics
{
    public class ReportTrackUnit
    {
        public string? TestName {  get; set; }
        public List<RecordUnit>? RecordUni { get; set; }
    }
}
