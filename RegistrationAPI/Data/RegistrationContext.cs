using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RegistrationAPI.Core.Models;

namespace RegistrationAPI.Data
{
    public class RegistrationContext : IdentityDbContext<ApplicationUser>
    {
        public RegistrationContext(DbContextOptions<RegistrationContext> options):base(options)
        {
            
        }
        public DbSet<Event> Events { get; set; }
        public DbSet<Registration> Registrations { get; set; }
    }
}
