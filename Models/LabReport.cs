using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class LabReport
    {
        public int Id { get; set; }
        public int PrescriptID { get; set; }
        public DateTime Time { get; set; }
        public  DateTime Date { get; set; } 
        public string TestName { get; set; } = null!;
        public string Status { get; set; } = null!;
    }
}