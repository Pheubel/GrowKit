using GrowKitApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace GrowKitApi.Contexts
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        public ApplicationContext()
        {
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<GrowKitStick> SensorSticks { get; set; }
        public DbSet<PlantPreset> Presets { get; set; }
    }
}
