using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrowKitApi.Entities
{
    /// <summary> An entity that keeps track of user's email registration requests.</summary>
    public class EmailConfirmationRequest
    {
        /// <summary> The id of the user requesting the email.</summary>
        [Key]
        public long UserId { get; set; }
        /// <summary> The token used to confirm the request.</summary>
        public Guid ConfirmationToken { get; set; }
        /// <summary> The date of when the request will expire.</summary>
        public DateTime ExpirationDate { get; set; }

        /// <summary> The user requesting the email.</summary>
        [ForeignKey("UserId")]
        public AuthenticationUser User { get; set; }
    }
}
