using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class User_Tele
    {
        public int Id { get; set; }
        public int Telephonenumber { get; set; }
        public User? User { get; set; }
    }
}