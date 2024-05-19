using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Models
{
    public class Admin
    {
        public int Id { get; set; }

        [ForeignKey("Id")]
        public int UserId { get; set; }
        [JsonIgnore]
        public User? User { get; set; }
    }
}
