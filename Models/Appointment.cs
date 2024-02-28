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
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public string Status { get; set; } = null!;

        [JsonIgnore]
        [InverseProperty("Patient")]
        public Patient? patient { get; set; }

        [JsonIgnore]
        [InverseProperty("User")]
        public User? doctor { get; set; }

        [JsonIgnore]
        [InverseProperty("Prescription")]
        public Prescription? prescription { get; set; }
    }
}
