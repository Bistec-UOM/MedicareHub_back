using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Models
{
    public class Appointment
    {
        [Key]
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public string? Status { get; set; } 


        [ForeignKey("Id")]
        public int PatientId { get; set; }
        [JsonIgnore]
        public Patient? Patient { get; set; }


        [ForeignKey("DoctorId")]
        public int DoctorId { get; set; }
        [JsonIgnore]
        public Doctor? Doctor { get; set; }


        [ForeignKey("RecepId")]
        public int RecepId { get; set; }
        [JsonIgnore]
        public Receptionist? Recep { get; set; }


        [JsonIgnore]
        public Prescription? Prescription { get; set; } = null;


    }
}