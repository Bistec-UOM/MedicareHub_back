using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Models
{
    public class Bill_drug
    {
        public int Id { get; set; }


        [ForeignKey("Id")]        
        public int DrugID { get; set; }
        [JsonIgnore]
        public Drug? Drug { get; set; }



        [ForeignKey("Id")]  
        public int PrescriptionID { get; set; }
        [JsonIgnore]
        public Prescription? Prescription { get; set; }
        

        public int Amount { get; set; }
    }
}
