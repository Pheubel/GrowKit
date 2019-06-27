using GrowKitApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace GrowKitApi.Contexts
{
    /// <summary> A context exposing authentication specific tables to the program.</summary>
    public class AuthenticationContext : DbContext
    {
        /// <summary> Creates a context instance with the provided options</summary>
        /// <param name="options"> The options to be used by the context instance.</param>
        public AuthenticationContext(DbContextOptions<AuthenticationContext> options) : base(options)
        {
        }

        /// <summary> Creates a context instance without additional options.</summary>
        public AuthenticationContext()
        {
        }

        /// <summary> </summary>
        public DbSet<AuthenticationUser> Users { get; set; }
        /// <summary> </summary>
        public DbSet<EmailConfirmationRequest> EmailConfirmationRequests { get; internal set; }
    }
}
