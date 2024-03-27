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
            var prescriptionData = await _cntx.prescriptions
                .Include(p => p.Appointment)
                    .ThenInclude(a => a.Patient)
                .Select(p => new
                {
                    id = p.Id,
                    time = p.DateTime.TimeOfDay.ToString(@"hh\:mm"),
                    Total = p.Total,
                    CashierId = p.CashierId,
                    name = p.Appointment.Patient.Name,
                    age = CalculateAge(p.Appointment.Patient.DOB ?? DateTime.MinValue),
                    gender = p.Appointment.Patient.Gender,
                    medicine = _cntx.prescript_Drugs
                        .Where(d => d.PrescriptionId == p.Id)
                        .Select(d => new
                        {
                            DrugId = d.Id,
                            name = d.GenericN,
                            quantity = d.Weight,
                            hour = d.Period
                        }).ToList()
                })
                .ToListAsync();

            return prescriptionData;
        }
        private int CalculateAge(DateTime dateOfBirth)
        {
            var today = DateTime.Today;
            var age = today.Year - dateOfBirth.Year;
            if (dateOfBirth.Date > today.AddYears(-age))
                age--;
            return age;
        }


    }
}
