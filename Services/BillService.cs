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
                try
                {
                    foreach (var item in data.Data)
                    {
                        var drug = await _cntx.drugs.FirstOrDefaultAsync(d => d.Id == item.DrugID);

                        if (drug == null)
                        {
                            throw new Exception($"Drug with ID {item.DrugID} not found.");
                        }

                        if (drug.Avaliable < item.Amount)
                        {
                            throw new Exception($"Not enough quantity for drug with ID {item.DrugID}. Available: {drug.Avaliable}, Requested: {item.Amount}");
                        }

                        // Reduce the quantity of the drug
                        drug.Avaliable -= item.Amount;
                        _cntx.drugs.Update(drug);

                        // Add the bill drug
                        await _cntx.bill_Drugs.AddAsync(item);
                    }

                    var prescription = await _cntx.prescriptions
                        .FirstOrDefaultAsync(e => e.Id == data.Data[0].PrescriptionID);

                    if (prescription == null)
                    {
                        throw new Exception($"Prescription with ID {data.Data[0].PrescriptionID} not found.");
                    }

                    prescription.Total = data.Total;
                    prescription.CashierId = 1;

                    var appointment = await _cntx.appointments
                        .FirstOrDefaultAsync(e => e.Id == prescription.AppointmentID);

                    if (appointment == null)
                    {
                        throw new Exception($"Appointment with ID {prescription.AppointmentID} not found.");
                    }

                    appointment.Status = "paid";

                    _cntx.prescriptions.Update(prescription);
                    _cntx.appointments.Update(appointment);

                    await _cntx.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception("An error occurred while adding bill drugs: " + ex.Message);
                }
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
