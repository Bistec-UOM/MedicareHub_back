using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO.Auth
{
    public class SentOTP
    {
        public int UserId { get; set; }
        public int OTP { get; set; }
    }
}
