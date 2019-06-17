using System.Threading.Tasks;

namespace GrowKitApi.Services
{
    /// <summary> Represents a service for sending email messages.</summary>
    public interface IEmailService
    {
        /// <summary> The email adress of the sender.</summary>
        string SenderAdress { get; }
        /// <summary> The server hosting the email service.</summary>
        string Host { get; }
        /// <summary> The port configured on the server for sending email messages.</summary>
        int Port { get; }
        /// <summary> Specifies if the Secule Socket Layer (SSL) will be used.</summary>
        bool EnableSsl { get; }

        /// <summary> Sends an email to to the destined adress.</summary>
        /// <param name="email"> The adress of the reciever.</param>
        /// <param name="subject"> The subject of the email message.</param>
        /// <param name="message"> The message that will be sent in the email.</param>
        /// <param name="isHtml"> Specifies if the message body utilizes HTML.</param>
        Task SendEmailAsync(string email, string subject, string message, bool isHtml = false);
    }
}
