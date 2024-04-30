using Models.DTO.Lab.ViewResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO.Lab.ViewResults
{
    public class VResult
    {
        public int ReportId { get; set; }
        public string TestName { get; set; } = null!;
        public DateTime DateTime { get; set; }
        public List<VResultField>? Results { get; set; }
    }
}
