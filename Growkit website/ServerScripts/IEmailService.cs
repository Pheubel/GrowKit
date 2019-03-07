using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Growkit_website.ServerScripts
{
    public interface IEmailService
    {
        string SenderAdress { get; }
        string Host { get; }
        int Port { get;}
        bool EnableSsl { get; }
        

        Task SendEmailAsync(string email, string subject, string Message, bool isHtml = false);
    }
}
