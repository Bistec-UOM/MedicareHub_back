using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Models
{
    public class Drug
    {
        public int Id { get; set; }
        public string GenericN { get; set; } = null!;
        public string BrandN { get; set; } = null!;
        public int Weight { get; set; }
        public float Price { get; set; }
        public string? Avaliable { get; set; }

        [JsonIgnore]
        public Bill_drug? Bill_drug { get; set; }
    }
}