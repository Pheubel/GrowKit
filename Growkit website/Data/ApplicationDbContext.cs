using System;
using System.Collections.Generic;
using System.Text;
using GrowkitDataModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Growkit_website.Data
{
    /// <summary> A database context that provides acces related to the growthkit.</summary>
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        /// <summary> </summary>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        /// <summary> The dataset containing the friend relationships between users.</summary>
        public DbSet<Friends> FriendRelations { get; set; }
        /// <summary> Contains all hubs registered in the system.</summary>
        public DbSet<GKHub> Hubs { get; set; }
        /// <summary> The dataset of all registeed sticks in the system.</summary>
        public DbSet<GKSensorStick> SensorSticks { get; set; }
        /// <summary> The dataset of all presets stored in the database.</summary>
        public DbSet<PlantPreset> Presets { get; set; }
    }
}
