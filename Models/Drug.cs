using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    internal class Drug
    {
        public string DrugID { get; set; }
        public string GenericN { get; set; }
        public string BrandN { get; set; }
        public float Weight { get; set; }
        public float Price { get; set; }
        public string Avaliable { get; set; }
    }
}
