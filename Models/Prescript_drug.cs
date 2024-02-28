using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
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