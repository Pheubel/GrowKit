using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GrowkitDataModels
{
    /// <summary> A model to keep track of user's friendship relations.</summary>
    public class Friends
    {
        /// <summary> The unique identity of this relation.</summary>
        [Key]public ulong RelationId { get; set; }
        /// <summary> Determines if the friend request has been accepted by the other user.</summary>
        public bool IsConfirmed { get; set; }

        /// <summary> The user that sent the friend request.</summary>
        public virtual ApplicationUser FriendFrom { get; set; }
        /// <summary> The User that recieved the friend request.</summary>
        public virtual ApplicationUser FriendTo { get; set; }
    }
}
