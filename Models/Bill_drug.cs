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
        public int Id { get; set; }
        public int DrugID { get; set; }
        public float Amount { get; set; }
    }
}
