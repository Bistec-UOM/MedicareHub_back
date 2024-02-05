using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Appointment
    {
        [Key]
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public string Status { get; set; } = null!;

        public int PatitenId { get; set; }

        public int DoctorId { get; set; }

        public int RecepId { get; set; }
    }
}
