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
