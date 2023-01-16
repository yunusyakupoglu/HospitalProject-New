using BL.IServices;
using MailKit.Security;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;


namespace BL.Services
{
    public class MailService : IMailService
    {
        public async Task<bool> SendAsync(string patientNameSurname, string doctorName, string appointmentDate, string appointmentTime, string note, string mailAdress)
        {
            try
            {
                var message = new MimeMessage();
                message.Bcc.Add(MailboxAddress.Parse(mailAdress));
                message.From.Add(MailboxAddress.Parse("ranesyapi@outlook.com"));

                message.Subject = "Müşteri Talebi";

                string messageBody = "<h3 style='color:red'>Hasta Adı Soyadı ..:</h3> <h4>" + patientNameSurname
                    + "</h4><h3 style='color:red'>Doktor Adı ..:</h3> <h4>" + doctorName
                    + "</h4><h3 style='color:red'>Randevu Tarihi ve Saati ..:</h3> <h4>" + appointmentDate + " " + appointmentTime
                    + "</h4><h3 style='color:red'>Doktor Notu ..:</h3> <h4>"
                    + note + "</h4>";
                //We will say we are sending HTML. But there are options for plaintext etc. 
                var builder = new BodyBuilder { HtmlBody = messageBody };

                message.Body = builder.ToMessageBody();


                using (var emailClient = new SmtpClient())
                {
                    //The last parameter here is to use SSL (Which you should!)
                    emailClient.Connect("smtp-mail.outlook.com", 587, SecureSocketOptions.StartTls);

                    //Remove any OAuth functionality as we won't be using it. 
                    emailClient.AuthenticationMechanisms.Remove("XOAUTH2");

                    emailClient.Authenticate("ranesyapi@outlook.com", "Ranes6020");

                    emailClient.Send(message);

                    emailClient.Disconnect(true);
                }

                return true;
            }
            catch (Exception e)
            {

                throw;
            }

        }
    }
}
