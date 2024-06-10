using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Otp
    {
        public int Id { get; set; }
        public int userId { get; set; }
        public int code { get; set; }
        public string? status { get; set; }
        public DateTime? DateTime { get; set; }
    }
}
