using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO.Lab.DoctorAnalytics
{
    public class RecordUnit
    {
        public string? Field { get; set; }
        public float MinRef { get; set; }
        public float MaxRef { get; set; }
        public string? Unit { get; set; }
        public float Result { get; set; }
        public string? Status { get; set; }
    }
}
