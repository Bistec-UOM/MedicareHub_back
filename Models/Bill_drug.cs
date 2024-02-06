using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    internal class Bill_drug
    {
        public string PrescriptID { get; set; }
        public string DrugID { get; set; }
        public float amount { get; set; }
    }
}
