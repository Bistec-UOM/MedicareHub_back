using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO.AdminDto
{
    public class A_Patient
    {
        public List<string> PatientDOBs { get; set; }
        public DateTime AppointmentDates { get; set; }
        public int OtherDetail1 { get; set; }
        public string OtherDetail2 { get; set; }
        // Add other details as needed
    }
}
