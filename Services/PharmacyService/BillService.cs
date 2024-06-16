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

namespace Services.PharmacyService
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
                .Where(p => p.Appointment.Status == "completed")
                .Select(p => new
                {
                    id = p.Id,
                    time = p.DateTime.TimeOfDay.ToString(@"hh\:mm"),
                    p.Total,
                    p.CashierId,
                    name = p.Appointment.Patient.Name,
                    age = CaluclateAge((DateTime)p.Appointment.Patient.DOB),
                    gender = p.Appointment.Patient.Gender,
                    medicine = _cntx.prescript_Drugs
                        .Where(d => d.PrescriptionId == p.Id && p.Appointment.Status == "completed")
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
                else
                {
                    List<Drug> emptyDrg = new List<Drug>();
                    medicineDetails.Add(name, emptyDrg);
                }
            }

            return medicineDetails;
        }
        public async Task<List<string>> GetMedicinesNotInStock(List<string> medicineNames)
        {
            var medicinesNotInStock = new List<string>();

            // Get the list of names that are available in the database
            var availableNames = await _cntx.drugs
                .Where(d => medicineNames.Contains(d.GenericN))
                .Select(d => d.GenericN)
                .ToListAsync();

            // Find names that are not in the availableNames list
            medicinesNotInStock = medicineNames.Except(availableNames).ToList();

            return medicinesNotInStock;
        }



        public async Task AddBillDrugs(Bill data,int roleId)
        {
            using (var transaction = await _cntx.Database.BeginTransactionAsync())
            {
                try
                {
                    foreach (var item in data.Data)
                    {
                        var drug = await _cntx.drugs.FirstOrDefaultAsync(d => d.Id == item.DrugID);

                        // Reduce the quantity of the drug
                        drug.Avaliable -= item.Amount;
                        _cntx.drugs.Update(drug);

                        // Add the bill drug
                        await _cntx.bill_Drugs.AddAsync(item);
                    }

                    var prescription = await _cntx.prescriptions
                        .FirstOrDefaultAsync(e => e.Id == data.PrescriptId);
                    prescription.Total = data.Total;
                    prescription.CashierId = roleId;

                    var appointment = await _cntx.appointments
                        .FirstOrDefaultAsync(e => e.Id == prescription.AppointmentID);
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
            if (now.Month < dob.Month || now.Month == dob.Month && now.Day < dob.Day)
            {
                age--;
            }
            return age;
        }

        //------------------------------------------Admin related------------------------------------------------
        public async Task<Notification> ReadNoti()
        {
            var drugAvailability = await _cntx.drugs
                .Where(d => d.Avaliable < 10)
                .Select(d => new
                {
                    Name = d.GenericN + "(" + d.Weight + "mg)",
                    Available = d.Avaliable
                })
                .ToListAsync();

            var pharmacistConnections = await _cntx.users
                .Where(u => u.Role == "Cashier" && u.ConnectionId != null)
                .Select(u => new
                {
                    connectionId = u.ConnectionId,
                    Id = u.Id
                })
                .ToListAsync();

            var unavailableDrugs = drugAvailability.Select(d => d.Name);
            string message = unavailableDrugs.Count() == 0
                ? ""
                : string.Join(", ", unavailableDrugs) + " drugs are less than 10 available";

            DateTime twentyFourHoursAgo = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, TimeZoneInfo.Local.Id, "Sri Lanka Standard Time").AddHours(-24);
            bool messageExists = await _cntx.notification
                .AnyAsync(n => n.Message == message && n.SendAt > twentyFourHoursAgo);
            Notification noti = new Notification();

            noti.Message = message;
            noti.SendAt = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Now, TimeZoneInfo.Local.Id, "Sri Lanka Standard Time"), TimeZoneInfo.Local.Id, "Sri Lanka Standard Time");
            noti.Seen = false;
            noti.From = "System";
            noti.To = 7.ToString();

            return noti;


           
        }




    }
}
