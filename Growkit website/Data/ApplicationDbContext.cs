using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Growkit_website.ServerScripts.Generators;
using GrowkitDataModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

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


        protected override void OnModelCreating(ModelBuilder builder)
        {
            var ulongConverter = new ValueConverter<ulong, long>(
                v => (long)v,
                v => (ulong)v);
            var ushortConverter = new ValueConverter<ushort, short>(
                v => (short)v,
                v => (ushort)v);

            builder.Entity<ApplicationUser>(table => 
            {
                table.Property(p => p.Id)
                    .HasValueGenerator(typeof(GUIDGenerator))
                    .ValueGeneratedOnAdd();
            });

            builder.Entity<PlantPreset>(table =>
            {
                table.Property("PresetId")
                    .ValueGeneratedOnAdd();

                table.Property(p => p.OptimalLightLevel)
                    .HasConversion(ushortConverter);

                table.Property(p => p.OptimalTemperatureLevel)
                    .HasConversion(ushortConverter);

                table.Property(p => p.OptimalWaterLevel)
                    .HasConversion(ushortConverter);
            });

            builder.Entity<Friends>(table =>
            {
                table.Property("RelationId")
                    .ValueGeneratedOnAdd();
            });

            builder.Entity<GKSensorStick>(table =>
            {
                table.Property("SensorId")
                    .ValueGeneratedOnAdd();

                table.Property(p => p.LightLevel)
                    .HasConversion(ushortConverter);

                table.Property(p => p.TemperatureLevel)
                    .HasConversion(ushortConverter);

                table.Property(p => p.WaterLevel)
                    .HasConversion(ushortConverter);
            });

            builder.Entity<GKHub>(table =>
            {
                table.Property("IMSI")
                    .HasConversion(ulongConverter);
            });

            base.OnModelCreating(builder);
        }
    }
}
