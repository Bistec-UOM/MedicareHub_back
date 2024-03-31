using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.DTO.Doctor;
using SendGrid.Helpers.Mail;
using System;
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
        public DoctorappoinmentService(IRepository<Appointment> appoinments, ApplicationDbContext context,
                                        IRepository<Prescription> pres,IRepository<Prescript_drug> psrdrg, IRepository<LabReport> labs)
        {
           _drug = psrdrg;
            _appoinments = appoinments;
            _context = context;
                _pres = pres;
            _labs = labs;
        }
       // get appoinment list from database
        public async Task<List<object>> GetPatientNamesForApp()
        {
            var tmp = _context.appointments

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
                    age = CaluclateAge((DateTime)a.Patient.DOB),
                    gender = a.Patient.Gender
                }
            })
            .ToList<object>();
            return tmp;
        }
        //funtion for calculate the age from DOB
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

        // for prescription
        public async Task<Prescription> AddPrescription(AddDrugs data)
        {
            
            var x = new Prescription
            {
                DateTime = DateTime.Now,
                AppointmentID = data.Id,
                description=data.Description,
                Total = 0,
                CashierId = 1
            };

            await _pres.Add(x);
            int pId = x.Id; //prescription id

            foreach (var i in data.Drugs)
            {
                var Obj= new Prescript_drug
                {
                    PrescriptionId = pId,
                    GenericN=i.GenericN,
                    Weight = i.Weight,
                    Unit = i.Unit,
                    Period = i.Period
                };
           
                await _drug.Add(Obj);
            }
           

            foreach (var d in data.Labs)
            {
                var Obj = new LabReport
                {
                    PrescriptionID = pId,
                    DateTime = null,    
                    TestId = 3,
                    Status = "New",
                    LbAstID = 1
                };
                await _labs.Add(Obj);

                var appointment = await _appoinments.Get(data.Id);
                if (appointment != null)
                {
                    appointment.Status = "completed";
                    await _appoinments.Update(appointment);
                }
                else
                {
                    // Handle the case when the appointment with the given ID is not found
                    throw new ArgumentException("Appointment not found");
                }               

            }
            return x;
        }

        }
    
    }
