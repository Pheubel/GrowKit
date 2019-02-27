﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GrowkitDataModels
{
    /// <summary> A subscriber model of a user of Vodaphone-Ziggo.</summary>
    public class User
    {
        /// <summary> The unique user identifying number.</summary>
        [Key] public Guid UserId { get; set; }
        /// <summary> The name displayed to other users.</summary>
        public string Username { get; set; }
        /// <summary> The hub currently registered on this user.</summary>
        /// <remarks>when going full scale, this should be added via inherritance or another method as multiple services might be added later.</remarks>
        public virtual ICollection<GKHub> OwnedHubs { get; set; }
    }
}