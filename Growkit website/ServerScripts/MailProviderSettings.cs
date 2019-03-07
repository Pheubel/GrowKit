using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Growkit_website.ServerScripts
{
    public class MailProviderSettings
    {
        public string SenderAdress { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public bool EnableSsl { get; set; }
    }
}
