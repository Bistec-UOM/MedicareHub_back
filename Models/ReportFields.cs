using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ReportFileds
    {
        public int Id { get; set; }
        public string TestName { get; set; } = null!;
        public string Fieldname { get; set; } = null!;
        public short Index { get; set; }
        public float MinRef { get; set; }
        public float MaxRef { get; set; }
        public string Unit { get; set; } = null!;
    }
}