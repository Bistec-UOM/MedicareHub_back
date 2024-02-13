using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Prescript_drug
    {
        public string Id { get; set; }

        public string GenericN { get; set; }

        public float Weight { get; set; }

        public int Period { get; set; }
    }
}