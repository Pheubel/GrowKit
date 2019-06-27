namespace GrowKitApi.SettingModels
{
    /// <summary> The settings for configureing a mail service.</summary>
    public class MailProviderSettings
    {
        /// <summary> The email adress of the sender.</summary>
        public string SenderAdress { get; set; }
        /// <summary> The username used for network credentials.</summary>
        public string Username { get; set; }
        /// <summary> The password used for network credentials.</summary>
        public string Password { get; set; }
        /// <summary> The server hosting the email service.</summary>
        public string Host { get; set; }
        /// <summary> The port configured on the server for sending email messages.</summary>
        public int Port { get; set; }
        /// <summary> Specifies if the Secule Socket Layer (SSL) will be used.</summary>
        public bool EnableSsl { get; set; }
    }
}
