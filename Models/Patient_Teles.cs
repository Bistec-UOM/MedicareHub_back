using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Patient_Teles
    {
        public int Id { get; set; }
        public int PatientId {  get; set; }
        public int telephonenumber { get; set; }
    }
}
