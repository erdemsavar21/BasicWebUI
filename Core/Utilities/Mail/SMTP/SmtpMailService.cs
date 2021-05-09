using System;
using System.Net.Mail;

namespace Core.Utilities.Mail.SMTP
{
    public class SmtpMailService : IMailService
    {
        public SmtpMailService()
        {
        }

        public void SendConfirmMail(string confirmUrl, string[] toMailAdresses)
        {
           

            string body = $"<h3>Welcome.. Please verify your Email : <a href=\"{confirmUrl}\">Verify</a> </h3>";

            SendMail(body, toMailAdresses, "Confirm Email");

        }

        public void SendResetPasswordMail(string confirmUrl, string[] toMailAdresses)
        {


            string body = $"<h3>Hallo..  <a href=\"{confirmUrl}\">Reset Password</a> </h3>";

            SendMail(body, toMailAdresses, "Reset Password");

        }


        public static void SendMail(string body, string[] toMailAdresses, string subject)
        {
            MailMessage mail = new MailMessage();
            
            SmtpClient smtpClient = new SmtpClient("smtp.mailtrap.io");
            mail.From = new MailAddress("e_savar@hotmail.com");
            foreach (var item in toMailAdresses)
            {
                mail.To.Add(item);
            }

            mail.Subject = subject;
            mail.Body = body;

            mail.IsBodyHtml = true;
            smtpClient.Port = 25;
            smtpClient.Credentials = new System.Net.NetworkCredential("94acd948748cd8", "1894fe3a68d938");
            smtpClient.Send(mail);
        }
    }
}
