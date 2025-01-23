using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Security;

namespace Survey.Contracts.Auth
{
    public class EmailServices(IOptions<MailSettings> mailSettings , ILogger<EmailServices> logger) : IEmailSender
    {
        private readonly MailSettings _mailSettings = mailSettings.Value;
        private readonly ILogger<EmailServices> _logger = logger;

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            //construct the message and it will goes for 
            var message = new MimeMessage
            {
                Sender = MailboxAddress.Parse(_mailSettings.Mail) ,
                Subject = subject,
            };
            // reciever
            message.To.Add(MailboxAddress.Parse(email));
            //message builder
            var builder = new BodyBuilder 
            { HtmlBody = htmlMessage };
            message.Body = builder.ToMessageBody();
            //conect to the server 
            using var smtp = new SmtpClient();
            _logger.LogInformation("Sending email to {email}", email);
            smtp.Connect(_mailSettings.Host , _mailSettings.Port , SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(message);
            smtp.Disconnect(true);
        }
    }
}
