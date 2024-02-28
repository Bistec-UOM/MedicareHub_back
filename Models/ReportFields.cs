using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Models
{
    public class ReportFields
    {
        public int Id { get; set; }
        public string Fieldname { get; set; } = null!;
        public short Index { get; set; }
        public float MinRef { get; set; }
        public float MaxRef { get; set; }
        public string Unit { get; set; } = null!;


        [ForeignKey("Id")]
        public int TestId { get; set; }
        [JsonIgnore]
        public Test? Test { get; set; }

        [ForeignKey("Id")]
        public int RecordId { get; set; }
        [JsonIgnore]
        public List<Record>? Record { get; set;}
    }
}