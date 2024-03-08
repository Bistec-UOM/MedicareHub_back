using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO.Lab
{
    public class TemplateObj
    {
        //Name of template:  Test object
        public int Id { get; set; }
        public string TestName { get; set; } = null!;
        public string? Abb { get; set; }
        public float Price { get; set; }
        public string Provider { get; set; } = null!;

        //Actual template : ReportFields object
        public List<ReportFields>? ReportFields { get; set; }

    }
}
