using Microsoft.AspNetCore.Mvc;
using Services;
using Services.AppointmentService;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly DoctorappoinmentService _appointments;
        public DoctorController(DoctorappoinmentService appoinments)
        {
            _appointments = appoinments;
        }

        [HttpGet("AppointList")]
        public async Task<ActionResult<List<Object>>> GetPatientNamesForApp()
        {
            var tmp=await _appointments.GetPatientNamesForApp();
            return Ok(tmp);
        }

        
    }
}
