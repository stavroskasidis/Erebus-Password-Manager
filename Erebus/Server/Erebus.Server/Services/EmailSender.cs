using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Erebus.Server.Services
{
    // This class is used by the application to send Email and SMS
    // when you turn on two-factor authentication in ASP.NET Identity.
    // For more details see this link http://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender
    {
        private readonly IConfigProvider ConfigProvider;

        public EmailSender (IConfigProvider configProvider)
        {
            ConfigProvider = configProvider;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var configuration = ConfigProvider.GetConfiguration().SmtpSettings;
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Erebus password manager", configuration.SenderAddress));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = email;
            emailMessage.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(configuration.Host, configuration.Port, configuration.UseSSL ? SecureSocketOptions.Auto : SecureSocketOptions.None ).ConfigureAwait(false);
                await client.SendAsync(emailMessage).ConfigureAwait(false);
                await client.DisconnectAsync(true).ConfigureAwait(false);
            }
        }
    }
}
