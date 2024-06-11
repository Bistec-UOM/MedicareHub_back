using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.DTO.Doctor;
using SendGrid.Helpers.Mail;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Services
{
    public class DoctorappoinmentService
    {
        private readonly IRepository<Appointment> _appoinments;
        private readonly ApplicationDbContext _context;
        private readonly IRepository<Prescription> _pres;
        private readonly IRepository<Prescript_drug> _drug;
        private readonly IRepository<LabReport> _labs;
        private readonly IRepository<Test> _tests;
        public DoctorappoinmentService(IRepository<Appointment> appoinments, ApplicationDbContext context,
                                        IRepository<Prescription> pres, IRepository<Prescript_drug> psrdrg,
                                        IRepository<LabReport> labs, IRepository<Test> tests)
        {
            _drug = psrdrg;
            _appoinments = appoinments;
            _context = context;
            _pres = pres;
            _labs = labs;
            _tests = tests;
        }

        // get appoinment list from database
        public async Task<object> GetAppointmentsAndTests()
        {
            var appointments = await _context.appointments
                .Where(a => a.Status == "new") // Filter appointments with status "new"                
                .Select(a => new
                {
                    id = a.Id,
                    date = a.DateTime.Date,
                    time = a.DateTime.TimeOfDay.ToString(@"hh\:mm"),
                    status = "pending",
                    Patient = new
                    {
                        name = a.Patient.Name,
                        age = CalculateAge((DateTime)a.Patient.DOB),
                        gender = a.Patient.Gender,
                        id = a.Patient.Id
                    },
                   
                })
                .ToListAsync();

            var tests = await _context.tests
                .Select(t => new
                {
                    TestId = t.Id,
                    labTestName = t.TestName
                })
                .ToListAsync();

            return new
            {
                Appointments = appointments,
                Tests = tests
            };
        }

        // Function to calculate the age from DOB
        private static int CalculateAge(DateTime dob)
        {
            DateTime now = DateTime.UtcNow;
            int age = now.Year - dob.Year;
            if (now.Month < dob.Month || (now.Month == dob.Month && now.Day < dob.Day))
            {
                age--;
            }
            return age;
        }


  //....................................................................................................................................
  //....................................................................................................................................
  //....................................................................................................................................

        // get appoinment list from database filter on doctorId and todays date
        public async Task<object> GetPatientNamesForApp2(int doctorId)
        {
            var today = DateTime.UtcNow.Date; // Get today's date

            var tmp = await _context.appointments

            .Where(a => a.Status == "new" && a.DoctorId == doctorId && a.DateTime.Date == today) // Filter appointments with status "new"                
            .Select(a => new
            {
                id = a.Id,
                date = a.DateTime.Date,
                time = a.DateTime.TimeOfDay.ToString(@"hh\:mm"),
                status = "pending",
                Patient = new
                {
                    name = a.Patient.Name,
                    age = CaluclateAge2((DateTime)a.Patient.DOB),
                    gender = a.Patient.Gender,
                    id = a.Patient.Id
                }
            })
            .ToListAsync();

            var tests = await _context.tests
                .Select(t => new
                {
                    id = t.Id,
                    name = t.TestName
                })
                .ToListAsync();

            return new
            {
                Appointments = tmp ,
                Tests = tests
            };
        }
        //funtion for calculate the age from DOB
        private static int CaluclateAge2(DateTime dob)
        {
            DateTime now = DateTime.UtcNow;
            int age = now.Year - dob.Year;
            if (now.Month < dob.Month || (now.Month == dob.Month && now.Day < dob.Day))
            {
                age--;
            }
            return age;
        }
 //......................................................................................................................................
 //........................................................................................................................................
 //......................................................................................................................................

        // for prescription
        public async Task<Appointment> AddPrescription(AddDrugs data)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                var x = new Prescription
                {
                    DateTime = DateTime.Now,
                    AppointmentID = data.Id,
                    description = data.Description,
                    Total = 0,
                    CashierId = 1
                };

                await _context.AddAsync(x);
                await _context.SaveChangesAsync();

                int pId = x.Id; //prescription id

                foreach (var i in data.Drugs)
                {
                    var Obj = new Prescript_drug
                    {
                        PrescriptionId = pId,
                        GenericN = i.GenericN,
                        Weight = i.Weight,
                        Unit = i.Unit,
                        Period = i.Period
                    };

                    await _context.prescript_Drugs.AddAsync(Obj);
                }


                foreach (var d in data.Labs)
                {
                    var Obj = new LabReport
                    {
                        PrescriptionID = pId,
                        DateTime = null,
                        AcceptedDate = null,
                        TestId = d.TestId,
                        Status = "new"
                    };
                    await _context.labReports.AddAsync(Obj);
                }
                await _context.SaveChangesAsync();

                // update appoinment patient status
                var appointment = await _appoinments.Get(data.Id);
                appointment.Status = "completed";
                await _appoinments.Update(appointment);

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return appointment;
            }
        }

        //get the patient history according to thier patient ids
        public async Task<List<PrescriptionWithDrugs>> PrescriptionByPatientId(int patientId)
        {
            var appointmentIds = await _context.appointments
                .Where(a => a.PatientId == patientId)
                .Select(a => a.Id)
                .ToListAsync();     // give that appoinment ids list of given patient id

            var prescriptions = await _context.prescriptions
                .Where(p => appointmentIds.Contains(p.AppointmentID))
                .ToListAsync();    // give the precription data for that appoinment ids

            var prescriptionIds = prescriptions.Select(p => p.Id).ToList();
            // get the prescription ids list according to the appoinment ids



            var prescriptionWithDrugsList = new List<PrescriptionWithDrugs>();

            foreach (var x in prescriptionIds)
            {
                var prescription = await _context.prescriptions
                    .FirstOrDefaultAsync(p => p.Id == x);

                var prescriptionDrugs = await _context.prescript_Drugs
                    .Where(pd => pd.PrescriptionId == x)
                    .ToListAsync();

                var relatedLabReports = await _context.labReports
                    .Where(lr => lr.PrescriptionID == x)
                    .ToListAsync();
                // Fetch test names for the related lab reports
                var labReportIds = relatedLabReports.Select(lr => lr.Id).ToList();

                var testNames = await _context.tests
                    .Where(t => labReportIds.Contains(t.Id))
                    .Select(t => t.TestName)
                    .ToListAsync();

                var prescriptDrugs = new PrescriptionWithDrugs
                {
                    Prescription = prescription,
                    Drugs = prescriptionDrugs,
                    LabReports = relatedLabReports,
                    TestNames = testNames
                };

                prescriptionWithDrugsList.Add(prescriptDrugs);
            }

            return prescriptionWithDrugsList;
        }


    }

}
