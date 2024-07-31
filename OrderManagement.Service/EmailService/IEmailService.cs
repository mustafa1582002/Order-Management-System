namespace OrderManagement.Service.EmailService
{
    public interface IEmailService
    {
        public void SendEmail(EmailSetting emailSetting);
    }
}
