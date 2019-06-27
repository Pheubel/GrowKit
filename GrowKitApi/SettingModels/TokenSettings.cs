namespace GrowKitApi.SettingModels
{
    /// <summary> The settings used for generating jwt tokens.</summary>
    public class TokenSettings
    {
        /// <summary> The secret used to sign tokens.</summary>
        public string Secret { get; set; }
        /// <summary> The issuer of the token.</summary>
        public string Issuer { get; set; }
        /// <summary>The intended audience ofthe token.</summary>
        public string Audience { get; set; }
        /// <summary> The acces expiration period in minutes.</summary>
        public int AccesExpiration { get; set; }
    }
}
