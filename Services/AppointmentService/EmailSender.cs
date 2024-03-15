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

        string akey= "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";




        public async Task SendMail(string subject,string toEmail,string userName,string message)
        {
            var apiKey = akey;
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
