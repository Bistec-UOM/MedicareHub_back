using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Services.AppointmentService
{
    public class EmailSender
    {

        public async Task SendMail(string subject,string toEmail,string userName,string message)
        {
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("chathuraishara63@gmail.com", "Medicare Hub");
           
            var to = new EmailAddress(toEmail, userName);
            var plainTextContent = message;
            var htmlContent = "";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }


    }
}
