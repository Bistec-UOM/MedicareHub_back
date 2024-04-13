using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillController : ControllerBase
    {
        private readonly BillService _billService;
        public BillController(BillService billService)
        {
            _billService = billService;
        }


        [HttpGet("DrugRequest")]
        public async Task<ActionResult<IEnumerable<object>>> GetPatientPrescriptionData()
        {
            var pe = await _billService.RequestList();
            return Ok(pe);
        }
        [HttpPost("GetMedicineDetails")]
        public async Task<ActionResult<IDictionary<string, List<Drug>>>> GetMedicineDetails([FromBody] List<string> medicineNames)
        {
            var medicineDetails = await _billService.GetMedicineDetails(medicineNames);
            if (medicineDetails == null || medicineDetails.Count == 0)
            {
                return NotFound();
            }
            return Ok(medicineDetails);
        }

    }
}
