using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Prescription
    {
        public int Id { get; set; }
        public int AppointID { get; set; }
        public float total { get; set; }
        public int CashierID { get; set; }
    }
}
