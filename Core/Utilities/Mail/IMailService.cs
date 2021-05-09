using System;
namespace Core.Utilities.Mail
{
    public interface IMailService
    {
        void SendConfirmMail(string confirmUrl, string[] toMailAdresses);
        void SendResetPasswordMail(string confirmUrl, string[] toMailAdresses);
    }
}
