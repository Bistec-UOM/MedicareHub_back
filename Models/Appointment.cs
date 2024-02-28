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
    public class Appointment
    {
        [Key]
        public int AppointId { get; set; }
        public DateTime Time { get; set; }
        public string Status { get; set; } = null!;

        [ForeignKey("PatientId")]
        public int PatientId { get; set; }
        [JsonIgnore]
        public Patient? patient { get; set; }

        [ForeignKey("UserId")]
        public int UserId { get; set; }
        [JsonIgnore]
        public User? user { get; set; }

        [ForeignKey("PriscriptId")]
        public int PriscriptId { get; set; }
        [JsonIgnore]
        public Prescription? prescription { get; set; }

        [ForeignKey("LapRepId")]
        public int LabRepId { get; set; }
        [JsonIgnore]
        public LabReport? labReport { get; set; }
    }
}
