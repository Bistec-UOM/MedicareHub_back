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
    public class DoctorAnalyticService: IDoctorAnalyticService
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

        public async Task<List<object>> TrackReportList(int id)
        {
            var prescriptions = await _cnt.prescriptions
             .Where(p => p.Appointment.PatientId == id)
             .OrderBy(p => p.DateTime)
             .ToListAsync();
            
            var categorizedReps = new List<object>();

            foreach (var i in prescriptions)
            {
                var lb =_cnt.labReports
                    .Where(l => l.PrescriptionID == i.Id && l.Status=="done")
                    .Include(l => l.Test)
                    .Select(l => new {
                            Id = l.Id,
                            LabRepDateTime = l.DateTime,
                            TestName = l.Test.TestName
                    })
                    .FirstOrDefault();

                categorizedReps.Add(lb);
            }

            return categorizedReps;

        }
    }
}
