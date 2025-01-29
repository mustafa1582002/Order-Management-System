using System.Net;
using System.Net.Mail;

namespace OrderManagement.Service.EmailService
{
    public class EmailService : IEmailService
    {
        public void SendEmail(EmailSetting emailSetting)
        {
            var client = new SmtpClient("smtp.gmail.com", 587);
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential("mustafa1582002@gmail.com", "hvboztixlvtxzncb");
            client.Send("mustafa1582002@gmail.com", emailSetting.TO, emailSetting.Title, emailSetting.Body);

        }
    }
}
    