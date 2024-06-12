using DataAccessLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.DTO;
using Services.DoctorService;
using Services.LabService;

namespace API.Controllers.DoctorControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistoryController : ControllerBase
    {
        private readonly DoctorAnalyticService _ans;
        private readonly ValueService _vs;
        private readonly DoctorappoinmentService _dApp;

        public HistoryController(DoctorAnalyticService ans, ValueService vs, DoctorappoinmentService dApp)
        {
            _ans = ans;
            _vs = vs;
            _dApp = dApp;
        }

        //Get (new lab Results + History Records + Analytics) at once
        [HttpGet("history")]
        public async Task<ActionResult<History>> RequestHistory(int Pid)
        {
            History tmp = new History();
            tmp.Lb = await _vs.CheckResult(Pid);
            tmp.Rec = await _dApp.PrescriptionByPatientId(Pid);
            tmp.Drgs = await _ans.TrackDrugList(Pid);
            tmp.Rprts = await _ans.TrackReportList(Pid);
            return Ok(tmp);
        }
    }
}
