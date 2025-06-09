using RegistrationAPI.Core.Interfaces;
using System.Net;
using System.Net.Mail;

namespace RegistrationAPI.Infrastructure.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration configuration;

        public EmailSender(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            var email = configuration["EmailSettings:Email"];
            var password = configuration["EmailSettings:Password"];

            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential(email, password),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(email),
                Subject = subject,
                Body = message,
                IsBodyHtml = true,
            };
            mailMessage.To.Add(toEmail);

            await client.SendMailAsync(mailMessage);
        }
    }
}
