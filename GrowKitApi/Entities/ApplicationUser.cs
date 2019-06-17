using System.ComponentModel.DataAnnotations;

namespace GrowKitApi.Entities
{
    public class ApplicationUser
    {
        [Key]
        public long UserId { get; set; }

        public string Username { get; set; }
        public string NormalizedUsername { get; set; }
    }
}
