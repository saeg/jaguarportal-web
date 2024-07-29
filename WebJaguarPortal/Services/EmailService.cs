using System.Net.Mail;
using System.Net;
using WebJaguarPortal.Repository.Interfaces;
using WebJaguarPortal.Models;

namespace WebJaguarPortal.Services
{
    public class EmailService
    {
        private readonly SettingsService settingsService;
        private const string footerMail = "<br><br>Best regards,<br><img src=\"data:image/png;base64, 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\" alt=\"Jaguar Portal\" />";

        public EmailService(SettingsService settingsService)
        {
            this.settingsService = settingsService;
        }

        internal void SendMail(string message, string subject, string destination)
        {   
            message = message.Replace(Environment.NewLine, "<br>");

            message += footerMail;

            var config = settingsService.Get();

            if (!config.SmtpPort.HasValue ||
                config.SmtpFrom == null ||
                config.SmtpAddress == null ||
                config.SmtpPassword == null ||
                config.SmtpUsername == null)
                throw new Exception("The SMTP has not been configured");

            string fromEmail = config.SmtpFrom;
            string username = config.SmtpUsername;
            string password = config.SmtpPassword;

            MailMessage mail = new(fromEmail, destination, subject, message)
            {
                IsBodyHtml = true
            };

            SmtpClient smtpClient = new(config.SmtpAddress, config.SmtpPort.Value)
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(username, password),
                EnableSsl = true
            };

            smtpClient.Send(mail);
        }
    }
}