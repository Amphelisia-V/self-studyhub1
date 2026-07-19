using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Configuration;

namespace SelfStudyHub2.API.Services
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(
            string toEmail,
            string subject,
            string body)
        {
            var emailSettings = _configuration
                .GetSection("EmailSettings");

            var senderEmail = emailSettings["Email"]!;
            var password = emailSettings["Password"]!;
            var host = emailSettings["Host"]!;
            var port = int.Parse(emailSettings["Port"]!);

            var email = new MimeMessage();

            email.From.Add(
                new MailboxAddress(
                    "Self Study Hub",
                   senderEmail
                )
            );

            email.To.Add(
                MailboxAddress.Parse(toEmail)
            );

            email.Subject = subject;

            email.Body = new TextPart("plain")
            {
                Text = body
            };


            using var smtp = new SmtpClient();

            await smtp.ConnectAsync(
                host,
                port,
                MailKit.Security.SecureSocketOptions.StartTls
            );


            await smtp.AuthenticateAsync(
               senderEmail,
                password
            );


            await smtp.SendAsync(email);

            await smtp.DisconnectAsync(true);
        }
    }
}
