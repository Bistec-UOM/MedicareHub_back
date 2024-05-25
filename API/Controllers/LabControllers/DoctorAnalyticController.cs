using DataAccessLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using SendGrid.Helpers.Mail;
using Services.LabService;

namespace API.Controllers.LabControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorAnalyticController : ControllerBase
    {

        private readonly DoctorAnalyticService Ans;
        private readonly ApplicationDbContext Cnt;

        public DoctorAnalyticController(DoctorAnalyticService ans, ApplicationDbContext cnt)
        {
            Ans = ans;
            Cnt = cnt;
        }

        [HttpGet]
        public async Task<ActionResult<Object>> RequestAnalyticData(int Id)
        {
            //Checking if the patient has ever arrived

            var tmp = await Cnt.prescriptions
            .AnyAsync(p => p.Appointment.PatientId == Id &&
                      Cnt.prescript_Drugs.Any(d => d.PrescriptionId == p.Id));

            if (tmp)
            {
                return Ok(await Ans.TrackDrugList(Id));
            }
            else 
            {
                return Ok("");
            }
            
        }

        [HttpGet("lab")]
        public async Task<ActionResult<Object>> RequestLabAnalyticData(int Id)
        {
            var tmp = await Cnt.prescriptions
            .AnyAsync(p => p.Appointment.PatientId == Id &&
                      Cnt.prescript_Drugs.Any(d => d.PrescriptionId == p.Id));

            if (tmp)
            {
                return Ok(await Ans.TrackReportList(Id));
            }
            else
            {
                return Ok("");
            }
        }


    }
}
