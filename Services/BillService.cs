using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.DTO;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
                .Where(p => p.Appointment.Status=="completed")
                .Select(p => new
                {
                    id = p.Id,
                    time = p.DateTime.TimeOfDay.ToString(@"hh\:mm"),
                    Total = p.Total,
                    CashierId = p.CashierId,
                    name = p.Appointment.Patient.Name,
                    age = CaluclateAge((DateTime)p.Appointment.Patient.DOB),
                    gender = p.Appointment.Patient.Gender,
                    medicine = _cntx.prescript_Drugs
                        .Where(d => d.PrescriptionId == p.Id && p.Appointment.Status=="completed")
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
        public async Task<IDictionary<string, List<Drug>>> GetMedicineDetails(List<string> medicineNames)
        {
            var medicineDetails = new Dictionary<string, List<Drug>>();

            foreach (var name in medicineNames)
            {
                var medicines = await _cntx.drugs
                    .Where(m => m.GenericN == name)
                    .ToListAsync();

                if (medicines != null && medicines.Any())
                {
                    medicineDetails.Add(name, medicines);
                }
            }

            return medicineDetails;
        }
        public async Task AddBillDrugs(Bill data) 
        {
            using (var transaction = await _cntx.Database.BeginTransactionAsync())
            {
                foreach (var item in data.Data)
                {
                    await _cntx.bill_Drugs.AddAsync(item);
                }

                Prescription tmp = await _cntx.prescriptions.Where(e => e.Id == data.Data[0].PrescriptionID).FirstOrDefaultAsync();
                tmp.Total = data.Total;
                tmp.CashierId = 1;

                Appointment tmp2 = await _cntx.appointments.Where(e => e.Id == tmp.AppointmentID).FirstOrDefaultAsync();
                tmp2.Status = "paid";

                _cntx.prescriptions.Update(tmp);
                _cntx.appointments.Update(tmp2);

                await _cntx.SaveChangesAsync();
                await transaction.CommitAsync();
            }
        }

        public async Task AddBillDrugs(Bill data)
        {
            using (var transaction = await _cntx.Database.BeginTransactionAsync())
            {
                foreach (var item in data.Data)
                {
                    await _cntx.bill_Drugs.AddAsync(item);
                }

                Prescription tmp=await _cntx.prescriptions.Where(e => e.Id == data.Data[0].PrescriptionID).FirstOrDefaultAsync();
                tmp.Total = data.Total;
                tmp.CashierId = 1;

                Appointment tmp2 =await _cntx.appointments.Where(e => e.Id==tmp.AppointmentID).FirstOrDefaultAsync();
                tmp2.Status = "paid";

                _cntx.prescriptions.Update(tmp);
                _cntx.appointments.Update(tmp2);

                await _cntx.SaveChangesAsync();
                await transaction.CommitAsync();
            }

        }

        private static int CaluclateAge(DateTime dob)
        {
            DateTime now = DateTime.UtcNow;
            int age = now.Year - dob.Year;
            if (now.Month < dob.Month || (now.Month == dob.Month && now.Day < dob.Day))
            {
                age--;
            }
            return age;
        }


    }
}
