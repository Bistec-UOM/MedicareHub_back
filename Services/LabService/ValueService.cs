using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.DTO.Lab.UploadResults;
using Models.DTO.Lab.ViewResults;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.LabService
{
    public class ValueService: IValueServices
    {
        private readonly IRepository<LabReport> _rep;
        private readonly IRepository<Record> _rec;
        private readonly IRepository<Test> _tst;
        private readonly ApplicationDbContext _cntx;
        public ValueService(IRepository<LabReport> rep,IRepository<Record> rec, IRepository<Test> tst, ApplicationDbContext contxt)
        {
            _rep = rep;
            _rec = rec;
            _tst = tst;
            _cntx = contxt;
        }


        async public Task<IEnumerable<Object>> RequestList()
        {
            var data=await _cntx.patients
               .Where(p => p.Appointment.Any(a => a.Prescription != null)) // Filter out patients without appointments or prescriptions
               .SelectMany(p => p.Appointment.Where(a => a.Prescription != null).Select(a => new
               {
                   date=1,
                   name = p.Name,
                   gender=p.Gender,
                   age=CaluclateAge((DateTime)p.DOB),
                   id = a.Prescription.Id,
                   dateTime = a.Prescription.DateTime,
                   load= _cntx.labReports
                        .Where(lr => lr.PrescriptionID == a.Prescription.Id && lr != null && lr.Status=="new") 
                        .Select(lr => new
                            {
                                    repId = lr.Id,
                                    testId=lr.TestId,
                                    test = _cntx.tests
                                            .Where(t => t.Id == lr.TestId)
                                            .Select(t => t.Abb)
                                            .FirstOrDefault(),
                                    testName = _cntx.tests
                                            .Where(t => t.Id == lr.TestId)
                                            .Select(t => t.TestName)
                                            .FirstOrDefault(),
                                    price = _cntx.tests
                                            .Where(t => t.Id == lr.TestId)
                                            .Select(t => t.Price)
                                            .FirstOrDefault()
                        }).ToList()
                    })).ToListAsync();

            data = data.Where(obj => obj.load.Count > 0).ToList();
            return data;
             
        }
        async public Task<Boolean> AcceptSample(int id)
        {
            var tmp= await _rep.Get(id);
            if (tmp != null)
            {
                tmp.Status = "accepted";
                tmp.AcceptedDate = DateTime.Now;
                await _rep.Update(tmp);
                return true;
            }
            return false;
        }

        async public Task<IEnumerable<Object>> AcceptedSamplesList()
        {
            return await _cntx.labReports
                .Where(lr=>lr.Status=="accepted")
                .Select(l => new
                    {
                        Id = l.Id,
                        TestId = l.TestId,
                        TestName = l.Test!.TestName,
                        Abb= l.Test.Abb,
                        Accepted = l.AcceptedDate
                     })
                .ToListAsync<object>();
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

        async public Task<Boolean> UplaodResults(Result data,int RoleId)
        {
            foreach (var i in data.Results!)
            {
                Record record = new Record
                {
                    LabReportId = data.ReportId,
                    ReportFieldId = i.Fieldid,
                    Result = i.Result,
                    Status = i.Status
                };

                await _cntx.AddAsync(record);
            }

            LabReport? tmp = await _cntx.labReports.FindAsync(data.ReportId);

            if (tmp == null)
            {
                throw new NullReferenceException("");
            }

            tmp.DateTime=DateTime.UtcNow;
            tmp.Status = "done";
            tmp.LbAstID = RoleId;
            _cntx.labReports.Update(tmp);
            await _cntx.SaveChangesAsync();
            return true;
        }

        //view result of a lab report when labReport Id is given
        private async Task<VResult> ViewResult(int id)
        {
            var labRep = await _cntx.labReports.FindAsync(id);
            var obj = new VResult();

            var tst = await _tst.Get(labRep.TestId);
    
            var records = await _cntx.records
                .Where(p => p.LabReportId == id)
                .ToListAsync();
    
            var fields = await _cntx.reportFields
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

        public async Task<List<VResult>> CheckResult(int Pid)
        {
            var appointmentIds = _cntx.appointments
                .Where(a => a.PatientId == Pid)
                .Select(a => a.Id)
                .ToList();

            var labReportIds = _cntx.labReports
                .Where(l => appointmentIds.Contains(l.Prescription.AppointmentID) && l.Status == "done")
                .Select(l => l.Id)
                .ToList();

            var labReports=new List<VResult>();

            foreach (var item in labReportIds)
            {
                labReports.Add(await ViewResult(item));
            }

            return labReports;
        }


        public async Task<Boolean> MarkCheck(int id)
        {
            var tmp = await _rep.Get(id);
            if(tmp.Status == "done" ) 
            {
                tmp.Status = "checked";
                await _rep.Update(tmp);
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
