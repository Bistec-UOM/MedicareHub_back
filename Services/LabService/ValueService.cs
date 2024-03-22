using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using Models;
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
        private readonly ApplicationDbContext _cntx;
        public ValueService(IRepository<LabReport> rep,IRepository<Record> rec,ApplicationDbContext contxt)
        {
            _rep = rep;
            _rec = rec;
            _cntx = contxt;
        }


        async public Task<IEnumerable<Object>> RequestList()
        {
            var data = await _cntx.patients
               .Where(p => p.Appointment.Any(a => a.Prescription != null)) // Filter out patients without appointments or prescriptions
               .SelectMany(p => p.Appointment.Where(a => a.Prescription != null).Select(a => new
               {
                   Name = p.Name,
                   Gender=p.Gender,
                   Age=CaluclateAge((DateTime)p.DOB),
                   PrescriptionId = a.Prescription.Id,
                   Lab= _cntx.labReports
                        .Where(lr => lr.PrescriptionID == a.Prescription.Id && lr != null) 
                        .Select(lr => new
                            {
                                    LabReportId = lr.Id,
                                    TestName = _cntx.tests
                                            .Where(t => t.Id == lr.TestId)
                                            .Select(t => t.TestName)
                                            .FirstOrDefault(),
                                    Price = _cntx.tests
                                            .Where(t => t.Id == lr.TestId)
                                            .Select(t => t.Price)
                                            .FirstOrDefault()
                        }).ToList()
                    })).ToListAsync();

            return data;
             
        }
        async public Task AcceptSample(int id)
        {
            var tmp= await _rep.Get(id);
            tmp.Status = "accepted";
            await _rep.Update(tmp);
        }

        async public Task<IEnumerable<LabReport>> AcceptedSamplesList()
        {
            return await _rep.GetByProp("Status", "accepted");
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

        async public Task UplaodResults(List<Record> data)
        {
            foreach (var i in data)
            {
                await _rec.Add(i);
            }
        }
    }
}
