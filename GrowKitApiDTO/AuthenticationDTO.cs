namespace GrowKitApiDTO
{
    /// <summary> The data model for authenticating a user.</summary>
    [System.Serializable]
    public class AuthenticationDTO
    {
        /// <summary> The application user's email adress used for registering an account and identifying themself.</summary>
        public string Email { get; set; }
        /// <summary> The application user's password used for authentication.</summary>
        public string Password { get; set; }
    }
}
