using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO
{
    public class AppointmentWithPatientDetails
    {
        public Appointment Appointment { get; set; }
        public User patient { get; set; }
    }
}
