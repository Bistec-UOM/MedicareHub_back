using Microsoft.AspNetCore.Mvc;
using Models;
using Services;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrescriptionController : ControllerBase
    {
        private readonly PrescriptionService _pres;
        public PrescriptionController(PrescriptionService pres)
        {
            _pres = pres;
        }
        [HttpPost]
        public async Task<ActionResult<string>> AddPrescription(Prescription item)
        {
            await _pres.AddPrescription(item);
            return Ok("Success");
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Prescription>>> GetPrescription()
        {
            var Prescriptions = await _pres.GetPrescription();
            return Ok(Prescriptions);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<string>> DeletePrescription(int id)
        {
            var result = await _pres.DeletePrescription(id);
            if (result)
                return Ok("Prescription deleted successfully.");
            else
                return NotFound("Prescription not found.");
        }
    }
}