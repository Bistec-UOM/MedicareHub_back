using Models;
using Models.DTO.AdminDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.AdminServices
{
    public interface IAnalyticsService
    {
        Task<List<A_Income>> GetAllAmount();
        Task<(List<string> PatientDOBs, List<DateTime> AppointmentDates)> GetAllPatientDetails();
    }
}
