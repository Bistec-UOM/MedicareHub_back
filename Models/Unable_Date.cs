using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Models
{
    public class Unable_Date
    {
        
        public int Id { get; set; }
        
        [ForeignKey("id")]
        public int doctorId { get; set; }
        [JsonIgnore]
        public Doctor? Doctor { get; set; }
        public DateTime Date { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set;}


    }
}