﻿using System;
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


        [ForeignKey("Id")]
        public int PrescriptionId { get; set; }
        [JsonIgnore]
        public Prescription? Prescription { get; set; }

        public int GenericN { get; set; }
        public float Weight { get; set; }
        public string? Period { get; set; }
    }
}