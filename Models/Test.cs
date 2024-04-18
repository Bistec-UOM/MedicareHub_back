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
    public class Test
    {
        public int Id { get; set; }
        public string TestName { get; set; } = null!;
        public string Abb { get; set; } = null!;
        public decimal Price { get; set; }
        public string Provider { get; set; } = null!;


        [JsonIgnore]
        public List<ReportFields>? ReportFields { get; set; }
        [JsonIgnore]
        public List<LabReport>? LabReport { get; set; }


    }
}