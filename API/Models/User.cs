using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Password { get; set; }
        public string? NIC { get; set; }
        public string? Role { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public string? Qualifications { get; set; }
    }
}
