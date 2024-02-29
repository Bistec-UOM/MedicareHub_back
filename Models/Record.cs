using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Models
{
    public class Record
    {
        public int Id { get; set; }


        [ForeignKey("Id")]
        public int LabReportId { get; set; }
        [JsonIgnore]
        public LabReport? LabReport { get; set; }


        [ForeignKey("Id")]
        public int ReportFieldId { get; set; }
        [JsonIgnore]
        public ReportFields? ReportField { get; set; }

        public float Result { get; set; }
        public string? Status { get; set; }
    }
}
