using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO.Lab.DoctorAnalytics
{
    public class DrugTrack
    {
        public DateTime Date { get; set; }
        public List<Prescript_drug>? Drug { get; set; }
    }
}
