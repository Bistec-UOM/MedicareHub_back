using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.AppointmentService
{
    public interface IEmailSender
    {
        public Task SendMail(string subject, string toEmail, string userName, string htmlContent);
    }
}
