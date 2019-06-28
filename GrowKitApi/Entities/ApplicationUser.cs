using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrowKitApi.Entities
{
    /// <summary> The user representation viewed from the application's perspective.</summary>
    public class ApplicationUser
    {
        /// <summary> The id of the user.</summary>
        [Key]
        public long UserId { get; set; }
        /// <summary> THe username of the user that will be displayed through the app.</summary>
        public string Username { get; set; }
        /// <summary> The normalized username used for comparisons.</summary>
        public string NormalizedUsername { get; set; }

        /// <summary> The reference to the authentication user via the Id.</summary>
        [ForeignKey("UserId")]
        public AuthenticationUser AuthenticationUser { get; set; }
    }
}
