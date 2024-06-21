using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Models.DTO
{
    public class AppointmentWithDoctorDetails
    {
        public Appointment appointment { get; set; }
        public User doctor { get; set; }
    }
}
