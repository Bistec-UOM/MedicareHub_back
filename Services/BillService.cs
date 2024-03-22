using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class BillService
    {
        private readonly ApplicationDbContext _cntx;
        public BillService(ApplicationDbContext cntx)
        {
            _cntx = cntx;
        }

        public async Task<IEnumerable<object>> RequestList()
        {
            var prescriptions = await _cntx.prescriptions.ToListAsync();

            var prescriptionData = prescriptions
                .Select(p => new
                {
                    Prescription = p,
                    Drugs = _cntx.prescript_Drugs
                        .Where(d => d.PrescriptionId == p.Id)
                        .Select(d => new
                        {
                            DrugId = d.Id,
                            GenericName = d.GenericN,
                            Weight = d.Weight,
                            Period = d.Period
                        }).ToList()
                });

            var formattedData = prescriptionData.Select(p => new
            {
                PrescriptionId = p.Prescription.Id,
                DateTime = p.Prescription.DateTime,
                AppointmentId = p.Prescription.AppointmentID,
                Total = p.Prescription.Total,
                CashierId = p.Prescription.CashierId,
                Drugs = p.Drugs
            });

            return formattedData;
        }


    }
}
