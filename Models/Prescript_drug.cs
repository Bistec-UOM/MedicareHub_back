using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Prescript_drug
    {
        public int Id { get; set; }

        public string GenericN { get; set; }

        public float Weight { get; set; }

        public String Period { get; set; }
    }
}