using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Patient
    {
        public int Id { get; set; }
        public string? NIC { get; set; }
        public string? Name { get; set; }
        public string? FullName { get; set; }
        public DateTime DOB { get; set; }
        public string? ContactNumber { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public string? Gender { get; set; }
    }
}
