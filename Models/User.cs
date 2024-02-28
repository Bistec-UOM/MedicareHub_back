﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
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
        public string? age { get; set; }

        [JsonIgnore]
        [InverseProperty("Appointment")]
        public ICollection<Appointment>? Appointments { get; set; }

    }
}
