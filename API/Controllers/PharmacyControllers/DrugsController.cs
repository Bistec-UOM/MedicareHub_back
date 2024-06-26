using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.PharmacyService;

namespace API.Controllers.PharmacyControllers
{
    [Authorize(Policy = "Cash")]
    [Route("api/[controller]")]
    [ApiController]
    public class DrugsController : ControllerBase
    {
        private readonly DrugsService _drg;
        public DrugsController(DrugsService drg)
        {
            _drg = drg;
        }

        /// <summary>
        /// Request to add a new drug to the sysytem
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<string>> AddDrug(Drug item)
        {
            var result = await _drg.AddDrug(item);
            if (!result)
            {
                return BadRequest("Drug already exists");
            }
            return Ok("Success");
        }

        /// <summary>
        /// Retrieve a list of all drug in the system
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Drug>>> GetDrugs()
        {
            var drugs = await _drg.GetDrugs();
            return Ok(drugs);
        }

        /// <summary>
        /// Requests to remove a drug from the system by its ID
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<string>> DeleteDrug(int id)
        {
            var result = await _drg.DeleteDrug(id);
            if (result)
                return Ok("Drug deleted successfully.");
            else
                return NotFound("Drug not found.");
        }

        /// <summary>
        /// Requests to retrieve a specific drug by its ID
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Drug>> GetDrug(int id)
        {
            var drug = await _drg.GetDrugById(id);
            if (drug == null)
                return NotFound("Drug not found.");

            return Ok(drug);
        }

        /// <summary>
        /// Requests to update an existing drug's details by its ID
        /// </summary>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<string>> UpdateDrug(int id, [FromBody] Drug updatedDrug)
        {
            var result = await _drg.UpdateDrug(id, updatedDrug);
            if (result)
                return Ok("Drug updated successfully.");
            else
                return NotFound("Drug not found.");
        }

        /// <summary>
        /// Retrieve the current service charge details
        /// </summary>
        /// <returns></returns>
        [HttpGet("servicecharge")]
        public ActionResult<ServiceCharge> GetServiceCharge()
        {
            var serviceCharge = _drg.GetServiceCharge();
            if (serviceCharge == null)
                return NotFound("Service charge not found.");

            return Ok(serviceCharge);
        }

        /// <summary>
        /// Requests to update the service charge details
        /// </summary>
        /// <returns></returns>
        [HttpPut("servicecharge")]
        public ActionResult<string> UpdateServiceCharge([FromBody] ServiceCharge updatedServiceCharge)
        {
            var result = _drg.UpdateServiceCharge(updatedServiceCharge);
            if (result)
                return Ok("Service charge updated successfully.");
            else
                return NotFound("Service charge not found.");
        }
    }

}
