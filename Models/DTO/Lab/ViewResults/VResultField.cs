using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO.Lab.ViewResults
{
    public class VResultField
    {
        public string Fieldname { get; set; } = null!;
        public float Value { get; set; }
        public float MinRef { get; set; }
        public float MaxRef { get; set; }
        public string Unit { get; set; } = null!;
        public string? Status {  get; set; }
    }
}
