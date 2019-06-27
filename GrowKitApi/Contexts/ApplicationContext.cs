using GrowKitApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace GrowKitApi.Contexts
{
    /// <summary> A context exposing application specific tables to the program.</summary>
    public class ApplicationContext : DbContext
    {
        /// <summary> Creates a context instance with the provided options</summary>
        /// <param name="options"> The options to be used by the context instance.</param>
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        /// <summary> Creates a context instance without additional options.</summary>
        public ApplicationContext()
        {
        }

        /// <summary> </summary>
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        /// <summary> </summary>
        public DbSet<GrowKitStick> SensorSticks { get; set; }
        /// <summary> </summary>
        public DbSet<PlantPreset> PlantPresets { get; set; }
    }
}
