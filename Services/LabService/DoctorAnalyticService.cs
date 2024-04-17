using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.DTO.Lab.DoctorAnalytics;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.LabService
{
    public class DoctorAnalyticService
    {
        private readonly ApplicationDbContext _cnt;
        public DoctorAnalyticService(ApplicationDbContext context) { 
            _cnt = context;
        }

        async public Task<List<Object>> TrackDrugList(int id)
        {
            var prescriptions =await _cnt.prescriptions
             .Where(p => p.Appointment.PatientId == id)
             .OrderBy(p => p.DateTime)
             .ToListAsync();

            var categorizedDrugs = new List<object>();

            foreach (var prescription in prescriptions)
            {
                var prescriptionDrugs =await _cnt.prescript_Drugs
                    .Where(d => d.PrescriptionId == prescription.Id)
                    .ToListAsync();

                var entry = new
                {
                    DateTime = prescription.DateTime,
                    Drugs = prescriptionDrugs
                };

                categorizedDrugs.Add(entry);
            }

            return categorizedDrugs;
        }
    }
}
