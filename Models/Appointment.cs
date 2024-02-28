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
        public string? Status { get; set; }

        [ForeignKey("Id")]
        public int PatientId { get; set; }
        [JsonIgnore]
        public Patient? Patient { get; set; }


        [ForeignKey("Id")]
        public int DoctorId { get; set; }
        [ForeignKey("Id")]
        public int RecepId { get; set; }
        [JsonIgnore]
        public User? User { get; set; }


    }
}