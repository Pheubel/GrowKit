using GrowKitApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace GrowKitApi.Contexts
{
    public class AuthenticationContext : DbContext
    {
        public AuthenticationContext(DbContextOptions<AuthenticationContext> options) : base(options)
        {
        }

        public AuthenticationContext()
        {
        }

        public DbSet<AuthenticationUser> Users { get; set; }
        public DbSet<EmailConfirmationRequest> EmailConfirmationRequests { get; internal set; }
    }
}
