using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Models
{
    public class Prescription
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }


        [ForeignKey("Id")]
        public int AppointmentID { get; set; }
        [JsonIgnore]
        public Appointment? Appointment { get; set; }


        [JsonIgnore]
        public List<Prescript_drug>? Prescript_drug { get; set; }

        [JsonIgnore]
        public List<LabReport>? LabReport { get; set; }

        [JsonIgnore]
        public List<Bill_drug>? Bill_drug { get;set; }


        public float Total { get; set; }


        [ForeignKey("CashierId")]
        public int CashierId { get; set; }
        [JsonIgnore]
        public User? Cashier { get; set; }

        [ForeignKey("LbAstID")]
        public int LbAstID { get; set; }
        [JsonIgnore]
        public User? LbAst { get; set; }

    }
}