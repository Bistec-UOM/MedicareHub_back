using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Prescription
    {
        public string Id { get; set; }

        public string AppointID { get; set; }

        public float total { get; set; }

        public string CashierID { get; set; }



    }
}
