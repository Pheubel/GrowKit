using GrowKitApi.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace GrowKitApi.Entities
{
    /// <summary> The user representation focused on authentication and sensitive data.</summary>
    public class AuthenticationUser
    {
        public AuthenticationUser()
        {
        }

        public AuthenticationUser(string email)
        {
            NormalizedEmailAdress = email.ToUpper();
        }

        /// <summary> The Id of the user.</summary>
        [Key]
        public long Id { get; set; }
        /// <summary> The uppercase representation fo the email adress.</summary>
        public string NormalizedEmailAdress { get; set; }
        /// <summary> The hash used for comparing passwords.</summary>
        public byte[] PasswordHash { get; set; }
        /// <summary> The generated salt used for hashing the password.</summary>
        public byte[] PasswordSalt { get; set; }
        /// <summary> The secret used for generating the authenticator code.</summary>
        public byte[] AuthenticatorSecret { get; set; }
        /// <summary> The last time window in which authentication with 2fa was attempted.</summary>
        public long PreviousTimeWindow { get; set; }
        /// <summary> The flags associated with the current user.</summary>
        public IdentityUserFlags Flags { get; internal set; }
    }
}
