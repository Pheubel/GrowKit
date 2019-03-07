using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Growkit_website.ServerScripts
{
    public class EmailService : IEmailService
    {
        readonly MailProviderSettings _mailSettings;

        public EmailService(IOptionsSnapshot<MailProviderSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public string SenderAdress => _mailSettings.SenderAdress;
        public string Host => _mailSettings.Host;
        public int Port => _mailSettings.Port;
        public bool EnableSsl => _mailSettings.EnableSsl;



        

        public async Task SendEmailAsync(string email, string subject, string Message, bool isHtml = false)
        {

            var smtpClient = new SmtpClient()
            {
                Host = _mailSettings.Host,
                Port = _mailSettings.Port,
                EnableSsl = _mailSettings.EnableSsl,
                Credentials = new NetworkCredential(_mailSettings.Username, _mailSettings.Password)
            };

            using (var message = new MailMessage(_mailSettings.SenderAdress, email, subject, Message)
            {
                IsBodyHtml = isHtml,
            })
            {
                await smtpClient.SendMailAsync(message);
            }

        }

        public Task SendEmailAsync(string email, string subject, string Message)
        {
            throw new NotImplementedException();
        }
    }
}
