using Models.DTO.Doctor;
using Models.DTO.Lab.ViewResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO
{
    public class History
    {
        public List<VResult>? Lb {  get; set; }
        public List<PrescriptionWithDrugs>? Rec { get; set; }
        public List<Object>? Drgs {  get; set; }
        public List<VResult>? Rprts { get; set; }
    }
}
