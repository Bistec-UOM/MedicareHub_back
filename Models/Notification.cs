using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Notification
    {
        public int Id { get; set; }
        public string? From { get; set; }
        public string? To { get; set; }
        public string? Message { get; set; }
        public DateTime? SendAt { get; set; }
    }
}

