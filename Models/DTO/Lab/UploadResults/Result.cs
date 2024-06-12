using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO.Lab.UploadResults
{
    public class Result
    {
        public int ReportId { get; set; }
        public DateTime DateTime { get; set; }
        public List<ResultField>? Results { get; set; }
        public Boolean Servere { get; set; }
    }
}
