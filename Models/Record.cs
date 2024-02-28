using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Models
{
    public class Record
    {
        public int Id { get; set; }
        [ForeignKey("Id")]
        public int ReportId { get; set; }
        [JsonIgnore]
        public LabReport? LabReport { get; set; }

        [JsonIgnore]
        public ReportFields? ReportFields { get; set; }
        public float Result { get; set; }
        public string Status { get; set; } = null!;
    }
}