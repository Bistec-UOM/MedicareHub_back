using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO
{
    public class Bill
    {
        public int PrescriptId { get; set; }
        public List<Bill_drug>? Data { get; set; }
        public float Total { get; set; }
    }
}