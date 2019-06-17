using GrowKitApi.SettingModels;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace GrowKitApi.Services
{
    /// <summary> A service responsible for sending email messages.</summary>
    public class EmailService : IEmailService
    {
        readonly MailProviderSettings _mailSettings;

        public EmailService(IOptions<MailProviderSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        /// <summary> The email adress of the sender.</summary>
        public string SenderAdress => _mailSettings.SenderAdress;
        /// <summary> The server hosting the email service.</summary>
        public string Host => _mailSettings.Host;
        /// <summary> The port configured on the server for sending email messages.</summary>
        public int Port => _mailSettings.Port;
        /// <summary> Specifies if the Secule Socket Layer (SSL) will be used.</summary>
        public bool EnableSsl => _mailSettings.EnableSsl;

        /// <summary> Sends an email to to the destined adress.</summary>
        /// <param name="email"> The adress of the reciever.</param>
        /// <param name="subject"> The subject of the email message.</param>
        /// <param name="message"> The message that will be sent in the email.</param>
        /// <param name="isHtml"> Specifies if the message body utilizes HTML.</param>
        public async Task SendEmailAsync(string email, string subject, string message, bool isHtml = false)
        {
            // Create a new client responsible for sending the mail message.
            using (var smtpClient = new SmtpClient()
            {
                Host = _mailSettings.Host,
                Port = _mailSettings.Port,
                EnableSsl = _mailSettings.EnableSsl,
                Credentials = new NetworkCredential(_mailSettings.Username, _mailSettings.Password)
            })
            // Construct the message
            using (var mailMessage = new MailMessage(_mailSettings.SenderAdress, email, subject, message)
            {
                IsBodyHtml = isHtml,
            })
            {
                await smtpClient.SendMailAsync(mailMessage);
            }

        }
    }
}
