using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Models
{
    public class Prescription
    {
        public int Id { get; set; }
        public string AppointID { get; set; }
        public float total { get; set; }
        public string CashierID { get; set; }

    }
}
