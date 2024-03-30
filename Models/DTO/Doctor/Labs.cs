using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO.Doctor
{
    public class Labs
    {
        public string DateTime { get; set; }
        public int TestId { get; set; }
        public string Status { get; set; }

        public int LbAstID { get; set; }
    }
}
