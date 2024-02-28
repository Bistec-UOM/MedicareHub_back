using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Drug
    {
        [Key]
        public int DrugId { get; set; }
        public string GenericN { get; set; } = null!;
        public string BrandN { get; set; } = null!;
        public float Weight { get; set; }
        public float Price { get; set; }
        public string Avaliable { get; set; }
    }
}
