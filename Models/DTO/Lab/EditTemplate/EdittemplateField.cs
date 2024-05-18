using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO.Lab.EditTemplate
{
    public class EdittemplateField
    {
        public int Id { get; set; }
        public string Fieldname { get; set; } = null!;
        public short Index { get; set; }
        public float MinRef { get; set; }
        public float MaxRef { get; set; }
        public string Unit { get; set; } = null!;
        public int TestId { get; set; }
        public String? Stat { get; set; }
    }
}
