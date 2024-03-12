using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO.AdminDto
{
    public class A_Patient
    {
        public A_Patient() { }
        public DateTime DateTime { get; set; }
        public int child_male { get; set; }
        public int child_female { get; set; }
        public int adult_male { get; set; }
        public int adult_female { get;set; }
        public int old_male { get; set; }   
        public int old_female { get; set; }
    }
}
