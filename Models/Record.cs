using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Record
    {
        public int Id { get; set; }
        public string FieldName { get; set; } = null!;
        public float Result { get; set; }
        public string Status { get; set; } = null!;
    }
}