using RegistrationAPI.Shared.DTOS.Dashboard;

namespace RegistrationAPI.Core.Interfaces
{
    public interface IDashboardService
    {
        Task<int> GetTotalRegistrationsAsync();
        Task<int> GetTotalEventsAsync();
        Task<int> GetTotalUsersAsync();
        Task<int> GetTotalOrganizersAsync();
        Task<MostRegisteredEventDto> GetMostRegisteredEventAsync();
        Task<Dictionary<string, int>> GetRegistrationsPerMonthAsync();
    }
}
