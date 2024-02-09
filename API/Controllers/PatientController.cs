using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        // GET: api/<PatientController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var patients = await _patientService.GetAllPatientsAsync();
            return Ok(patients);
        }

        // GET api/<PatientController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var patient = await _patientService.GetPatientAsync(id);

            if (patient == null)
            {
                return NotFound();
            }

            return Ok(patient);
        }

        // POST api/<PatientController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Patient value)
        {
            await _patientService.AddPatientAsync(value);
            return Ok(); // Assuming a successful operation
        }

        // PUT api/<PatientController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Patient value)
        {
            // Assuming you want to return a 404 if the patient doesn't exist
            if (await _patientService.GetPatientAsync(id) == null)
            {
                return NotFound();
            }

            await _patientService.UpdatePatientAsync(value);
            return Ok(); // Assuming a successful operation
        }

        // DELETE api/<PatientController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            // Assuming you want to return a 404 if the patient doesn't exist
            if (await _patientService.GetPatientAsync(id) == null)
            {
                return NotFound();
            }

            _patientService.DeletePatient(id);
            return Ok(); // Assuming a successful operation
        }
    }
}
