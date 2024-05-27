using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO.Auth
{
    public class NewPassword
    {
        public int UserId { get; set; }
        public string Password { get; set; } = null!;
    }
}
