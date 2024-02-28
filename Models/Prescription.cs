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
        [Key]
        public int PrescriptId { get; set; }
        public string AppointID { get; set; }
        public float total { get; set; }
        public string CashierID { get; set; }

        [JsonIgnore]
        public Appointment? appointment { get; set; }

        [JsonIgnore]
        [InverseProperty("Prescript_drug")]
        public I
    }
}
