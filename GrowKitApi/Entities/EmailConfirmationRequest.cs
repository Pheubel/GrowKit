using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrowKitApi.Entities
{
    public class EmailConfirmationRequest
    {
        [Key]
        public long UserId { get; set; }
        public Guid ConfirmationToken { get; set; }
        public DateTime ExpirationDate { get; set; }

        [ForeignKey("UserId")]
        public AuthenticationUser User { get; set; }
    }
}
