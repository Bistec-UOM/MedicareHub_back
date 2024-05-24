using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Models
{
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? FullName { get; set; }
        public string? Password { get; set; }
        public string? NIC { get; set; }
        public string? Role { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public string? Qualifications { get; set; }
        public string? ContactNumber { get; set; }
        public string? Email { get; set; }
        public Boolean IsDeleted {  get; set; }
        public string? ImageUrl { get; set; }
        public DateTime? DOB { get; set; }
        public string? ConnectionId { get; set; }

        [JsonIgnore]
        public List<User_Tele>? User_Tele { get; set; }
        [JsonIgnore]
        public Doctor? Doctor {  get; set; }
        [JsonIgnore]
        public Receptionist? Receptionist { get; set; }
        [JsonIgnore]
        public Cashier? Cashier { get; set; }
        [JsonIgnore]
        public LabAssistant? LabAssistant { get; set; }
    }
}