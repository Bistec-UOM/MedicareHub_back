using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Models
{
    public class Patient
    {
        public int Id { get; set; }
        public string? NIC { get; set; }
        public string? Name { get; set; }
        public string? FullName { get; set; }
        public string? DOB { get; set; }
        public string? ContactNumber { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public string? Gender { get; set; }

        [JsonIgnore]
        public List<Appointment>? Appointment { get; set; }

        [JsonIgnore]
        public List<Patient_Teles>? Patient_Teles { get; set; }
    }
}
