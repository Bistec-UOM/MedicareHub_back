using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Models
{
    public class Patient_Teles
    {
        public int Id { get; set; }
        public int Telephonenumber { get; set; }
        [JsonIgnore]
        public Patient? Patient { get; set; }
    }
}
