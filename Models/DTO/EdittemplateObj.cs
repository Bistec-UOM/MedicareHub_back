using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO
{
    public class EdittemplateObj
    {
        public int TestId { get; set; }
        public IEnumerable<int> Del_id { get; set; }
        public List<ReportFields> EditedFields { get; set; }
        public List<ReportFields> AddedFields { get; set; }
    }
}
