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
    public class LabReport
    {
        [Key]
        public int LabRepId { get; set; }
        public string PrescriptID { get; set; } = null!;
        public DateTime Time { get; set; }
        public  DateTime Date { get; set; } 
        public string TestName { get; set; } = null!;
        public string Status { get; set; } = null!;

        [JsonIgnore]
        [InverseProperty("Prescription")]
        public Prescription? prescription { get; set; }
    }
}