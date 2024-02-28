using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Bill_drug
    {
        public string Id { get; set; }
        public string DrugID { get; set; }
        public float amount { get; set; }
    }
}
