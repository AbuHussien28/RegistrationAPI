using Microsoft.EntityFrameworkCore;
using RegistrationAPI.Core.Interfaces;
using RegistrationAPI.Data;
using RegistrationAPI.Shared.DTOS.Dashboard;

namespace RegistrationAPI.Infrastructure.Repositorys
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly RegistrationContext context;

        public DashboardRepository(RegistrationContext context)
        {
            this.context = context;
        }

        public async Task<int> GetTotalRegistrationsAsync()
        {
            return await context.Registrations.CountAsync();
        }

        public async Task<int> GetTotalEventsAsync()
        {
            return await context.Events.CountAsync(e => !e.IsDeleted);
        }

        public async Task<int> GetTotalUsersAsync()
        {
            return await context.Users.CountAsync();
        }

        public async Task<int> GetTotalOrganizersAsync()
        {
            var organizerRole = await context.Roles
                .Where(r => r.Name == "Organizer")
                .Select(r => r.Id)
                .FirstOrDefaultAsync();

            return await context.UserRoles
                .CountAsync(ur => ur.RoleId == organizerRole);
        }

        public async Task<MostRegisteredEventDto> GetMostRegisteredEventAsync()
        {
            var result = await context.Registrations
                .GroupBy(r => r.EventId)
                .OrderByDescending(g => g.Count())
                .Select(g => new
                {
                    EventId = g.Key,
                    Count = g.Count()
                })
                .FirstOrDefaultAsync();

            if (result == null)
            {
                return new MostRegisteredEventDto
                {
                    Name = "No Event",
                    Registrations = 0,
                    StartDate = DateTime.MinValue,
                    Location = "N/A"
                };
            }

            var eventDetails = await context.Events
                .Where(e => e.EventId == result.EventId)
                .Select(e => new
                {
                    e.Title,
                    e.StartDate,
                    e.Location
                })
                .FirstOrDefaultAsync();

            return new MostRegisteredEventDto
            {
                Name = eventDetails?.Title ?? "No Event",
                Registrations = result.Count,
                StartDate = eventDetails?.StartDate ?? DateTime.MinValue,
                Location = eventDetails?.Location ?? "N/A"
            };
        }

        public async Task<Dictionary<string, int>> GetRegistrationsPerMonthAsync()
        {
            return await context.Registrations
                .GroupBy(r => new { r.RegistrationDate.Year, r.RegistrationDate.Month })
                .Select(g => new
                {
                    Month = $"{g.Key.Month}/{g.Key.Year}",
                    Count = g.Count()
                })
                .ToDictionaryAsync(x => x.Month, x => x.Count);
        }

        public async Task<Dictionary<string, int>> GetLastMonthRegistrationsByRoleAsync()
        {
            var lastMonth = DateTime.Now.AddMonths(-1);
            var roles = await context.Roles.ToDictionaryAsync(r => r.Id, r => r.Name);

            var query = from r in context.Registrations
                        join u in context.Users on r.UserId equals u.Id
                        join ur in context.UserRoles on u.Id equals ur.UserId
                        where r.RegistrationDate.Month == lastMonth.Month && r.RegistrationDate.Year == lastMonth.Year
                        group r by roles[ur.RoleId] into g
                        select new { Role = g.Key, Count = g.Count() };

            return await query.ToDictionaryAsync(x => x.Role, x => x.Count);
        }

    }
}
