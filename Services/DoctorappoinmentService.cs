using DataAccessLayer;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class DoctorappoinmentService
    {
        private readonly IRepository<Appointment> _appoinments;
        public DoctorappoinmentService(IRepository<Appointment> appoinments) 
        {
            _appoinments = appoinments;
        }
        
        public async Task<List<Appointment>> GetAllAppointments()
            {
                return await _appoinments.GetAll();
            }
        }
    
}
