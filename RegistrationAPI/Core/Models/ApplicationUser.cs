using Microsoft.AspNetCore.Identity;
using Stripe.Tax;

namespace RegistrationAPI.Core.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string FullName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
        public string? CurrentToken { get; set; }
        public DateTime? TokenExpiration { get; set; }
        public virtual ICollection<Event> CreatedEvents { get; set; }
        public virtual ICollection<Registration> Registrations { get; set; }
    }
}
