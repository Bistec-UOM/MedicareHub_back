using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.DTO.Doctor;
using Services.AppointmentService;
using Services.DoctorService;

namespace API.Controllers.DoctorControllers
{
    [Authorize(Policy = "Doct")]
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
        public async Task<ActionResult<List<object>>> GetPatientNamesForApp()
        {
            var tmp = await _appointments.GetAppointmentsAndTests();
            return Ok(tmp);
        }

        //....................................................................................................................................
        //........................................................................................................................................
        //....................................................................................................................................

        [HttpGet("AppointList2")]
        public async Task<ActionResult<List<object>>> GetPatientNamesForApp2()
        {
            var claim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "RoleId")?.Value;
            int roleId = int.Parse(claim);

            var tmp = await _appointments.GetPatientNamesForApp2(roleId);
            return Ok(tmp);
        }
        //....................................................................................................................................
        //........................................................................................................................................
        //....................................................................................................................................

        [HttpPost("Prescription")]
        public async Task<ActionResult<Appointment>> AddPrescription(AddDrugs data)
        {
            var tmp = await _prescription.AddPrescription(data);
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
