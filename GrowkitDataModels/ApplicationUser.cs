using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GrowkitDataModels
{
    /// <summary> A subscriber model of a user of Vodaphone-Ziggo.</summary>
    public class ApplicationUser : IdentityUser<Guid>
    {
        /// <summary> The hub currently registered on this user.</summary>
        /// <remarks> when going full scale, this should be added via inherritance or another method as multiple services might be added later.</remarks>
        public virtual ICollection<GKHub> OwnedHubs { get; set; }
    }
}
