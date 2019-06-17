using GrowKitApi.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace GrowKitApi.Entities
{
    public class IdentityUser
    {
        public IdentityUser(string email)
        {
            NormalizedEmailAdress = email.ToUpper();
        }

        [Key]
        public long Id { get; set; }
        public string NormalizedEmailAdress { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        public byte[] AuthenticatorSecret { get; set; }
        public long PreviousTimeWindow { get; set; }
        public IdentityUserFlags Flags { get; internal set; }
    }
}
