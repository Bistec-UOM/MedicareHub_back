using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO.Lab.DoctorAnalytics
{
    public class ReportTrack
    {
        public DateTime? Date { get; set; }
        public List<ReportTrackUnit>? ReportUnit { get; set; }
    }
}
