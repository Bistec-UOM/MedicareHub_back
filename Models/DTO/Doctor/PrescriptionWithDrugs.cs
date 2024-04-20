using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO.Doctor
{
    public class PrescriptionWithDrugs
    {
        public Prescription Prescription { get; set; }
        public List<Prescript_drug> Drugs { get; set; }
    }
}
