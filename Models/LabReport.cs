using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Models
{
    public class LabReport
    {

        public int Id { get; set; }

        [ForeignKey("Id")]
        public int PrescriptionID { get; set; }
        [JsonIgnore]
        public Prescription? Prescription { get; set; }


        public DateTime DateTime { get; set; }


        [ForeignKey("Id")]
        public int TestId { get; set; }
        [JsonIgnore]
        public Test? Test { get; set; }


        [JsonIgnore]
        public List<Record>? Records { get; set; }


        public string Status { get; set; } = null!;


        [ForeignKey("LbAstID")]
        public int LbAstID { get; set; }
        [JsonIgnore]
        public LabAssistant? LbAst { get; set; }
    }
}