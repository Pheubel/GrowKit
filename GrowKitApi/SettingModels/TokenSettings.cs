namespace GrowKitApi.SettingModels
{
    public class TokenSettings
    {
        public string Secret { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        /// <summary> The acces expiration period in minutes.</summary>
        public int AccesExpiration { get; set; }
    }
}
