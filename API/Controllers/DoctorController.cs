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

        [HttpGet]
        public async Task<IActionResult> GetAllAppointments()
        {
            var appointments = await _appointments.GetAllAppointments();
            return Ok(appointments);
        }
    }
}
