using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GrowkitDataModels
{
    /// <summary> A model to keep track of user's friendship relations.</summary>
    public class Friends
    {
        /// <summary> The unique identity of this relation.</summary>
        [Key] public ulong RelationId { get; set; }
        /// <summary> Determines if the friend request has been accepted by the other user.</summary>
        public bool IsConfirmed { get; set; }
        /// <summary> The Id of the user that sent the friend request.</summary>
        public Guid RequestSenderId { get; set; }
        /// <summary> The Id of the user that recieved the friend request.</summary>
        public Guid RequestRecieverId { get; set; }

        /// <summary> The user that sent the friend request.</summary>
        [ForeignKey("RequestSenderId")] public virtual ApplicationUser RequestSender { get; set; }
        /// <summary> The user that recieved the friend request.</summary>
        [ForeignKey("RequestRecieverId")] public virtual ApplicationUser RequestReciever { get; set; }
    }
}
