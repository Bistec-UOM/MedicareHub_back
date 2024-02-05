using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Test
    {
        public int Id { get; set; }
        public string TestName { get; set; } = null!;
        public float Price { get; set; }
        public string Provider { get; set; } = null!;
    }
}