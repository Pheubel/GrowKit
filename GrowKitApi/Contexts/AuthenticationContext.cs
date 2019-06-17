using GrowKitApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace GrowKitApi.Contexts
{
    public class IdentityContext : DbContext
    {
        public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
        {
        }

        public IdentityContext()
        {
        }

        public DbSet<IdentityUser> Users { get; set; }
        public DbSet<EmailConfirmationRequest> EmailConfirmationRequests { get; internal set; }
    }
}
