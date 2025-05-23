using Microsoft.EntityFrameworkCore;
using RegistrationAPI.Core.Interfaces;
using RegistrationAPI.Core.Models;
using RegistrationAPI.Data;

namespace RegistrationAPI.Infrastructure.Repositorys
{
    public class RegistrationRepository : GenericRepository<Registration>, IRegistrationRepository
    {
        private readonly RegistrationContext context;

        public RegistrationRepository(RegistrationContext context):base(context)
        {
            this.context = context;
        }
        public async Task<List<Registration>> GetAllByEventIdAsync(int eventId)
        {
            return await context.Registrations
               .Where(r => r.EventId == eventId)
               .ToListAsync();
        }

        public async Task<List<Registration>> GetAllByUserIdAsync(string userId)
        {
            return await context.Registrations
                 .Where(r => r.UserId == userId)
                 .ToListAsync();
        }

        public async Task<Registration> GetByUserAndEventAsync(string userId, int eventId)
        {
            return await context.Registrations
               .FirstOrDefaultAsync(r => r.UserId == userId && r.EventId == eventId);
        }
    }
}
