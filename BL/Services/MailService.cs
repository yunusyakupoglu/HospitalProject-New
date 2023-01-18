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
                message.To.Add(MailboxAddress.Parse(mailAdress));
                message.From.Add(MailboxAddress.Parse("hastaneotomasyonu81@outlook.com"));

                message.Subject = "Doktor Notu Talebi";

                string messageBody = "<p>Hasta Adı Soyadı ..: " + patientNameSurname
                    + "</p><p>Doktor Adı ..: " + doctorName
                    + "</p><p>Randevu Tarihi ve Saati ..: " + appointmentDate + " " + appointmentTime
                    + "</p><p>Doktor Notu ..:</p> <p>"
                    + note + "</p>";
                //We will say we are sending HTML. But there are options for plaintext etc. 
                var builder = new BodyBuilder { HtmlBody = messageBody };

                message.Body = builder.ToMessageBody();


                using (var emailClient = new SmtpClient())
                {
                    //The last parameter here is to use SSL (Which you should!)
                    emailClient.Connect("smtp-mail.outlook.com", 587, SecureSocketOptions.StartTls);

                    //Remove any OAuth functionality as we won't be using it. 
                    emailClient.AuthenticationMechanisms.Remove("XOAUTH2");

                    emailClient.Authenticate("hastaneotomasyonu81@outlook.com", "Hastane8181**");

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
