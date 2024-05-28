using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTO.Doctor;
using Services;
using Services.AppointmentService;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly DoctorappoinmentService _appointments;
        private readonly DoctorappoinmentService _prescription;
        public DoctorController(DoctorappoinmentService appoinments, DoctorappoinmentService prescription)
        {
            _appointments = appoinments;
            _prescription = prescription;
        }

        [HttpGet("AppointList")]
        public async Task<ActionResult<List<Object>>> GetPatientNamesForApp()
        {
            var tmp=await _appointments.GetPatientNamesForApp();
            return Ok(tmp);
        }
        
        //....................................................................................................................................
        //........................................................................................................................................
        //....................................................................................................................................

        [HttpGet("AppointList2/{doctorId}")]
        public async Task<ActionResult<List<Object>>> GetPatientNamesForApp2(int doctorId)
        {
            var tmp = await _appointments.GetPatientNamesForApp2(doctorId);
            return Ok(tmp);
        }        
        //....................................................................................................................................
        //........................................................................................................................................
        //....................................................................................................................................

        [HttpPost("Prescription")]
        public async Task<ActionResult<Appointment>> AddPrescription(AddDrugs data)
        {
            var tmp=await _prescription.AddPrescription(data);
            return Ok(tmp);
        }


        [HttpGet("prescription/{patientId}")]
        public async Task<ActionResult> PrescriptionByPatientId(int patientId)
        {
           var prescriptionDrugs = await _appointments.PrescriptionByPatientId(patientId);
           return Ok(prescriptionDrugs);           
          
        }

    }
}
