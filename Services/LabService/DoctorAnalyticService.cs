using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.DTO.Lab.DoctorAnalytics;
using Models.DTO.Lab.ViewResults;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Services.LabService;

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

        //view result of a lab report when labReport Id is given
        private async Task<VResult> ViewResult(int id)
        {
            var labRep = await _cnt.labReports.FindAsync(id);
            var obj = new VResult();

            var tst = await _cnt.tests.FindAsync(labRep.TestId);

            var records = await _cnt.records
                .Where(p => p.LabReportId == id)
                .ToListAsync();

            var fields = await _cnt.reportFields
                .Where(p => p.TestId == labRep.TestId)
                .ToListAsync();

            var resultList = fields
             .Join(records, field => field.Id, record => record.ReportFieldId,
                    (field, record) => new VResultField
                    {
                        Fieldname = field.Fieldname,
                        MinRef = field.MinRef,
                        MaxRef = field.MaxRef,
                        Value = record.Result,
                        Unit = field.Unit,
                        Status = record.Status
                    }
                    ).ToList();

            obj.ReportId = id;
            obj.TestName = tst.TestName;
            obj.DateTime = (DateTime)labRep.DateTime;
            obj.Results = resultList;

            return obj;
        }

        public async Task<List<VResult>> TrackReportList(int id)
        {
            var appointmentIds = _cnt.appointments
                .Where(a => a.PatientId == id)
                .Select(a => a.Id)
                .ToList();

            var labReportIds = _cnt.labReports
                .Where(l => appointmentIds.Contains(l.Prescription.AppointmentID) && (l.Status == "done"||l.Status=="checked"))
                .Select(l => l.Id)
                .ToList();

            var labReports = new List<VResult>();

            foreach (var item in labReportIds)
            {
                labReports.Add(await ViewResult(item));
            }

            return labReports;

        }
    }
}
