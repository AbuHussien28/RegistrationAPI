using RegistrationAPI.Core.Interfaces;
using RegistrationAPI.Infrastructure.Repositorys;
using RegistrationAPI.Shared.DTOS.Dashboard;

namespace RegistrationAPI.Infrastructure.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository dashboardRepository;

        public DashboardService(IDashboardRepository dashboardRepository)
        {
            this.dashboardRepository = dashboardRepository;
        }
        public Task<int> GetTotalRegistrationsAsync() => dashboardRepository.GetTotalRegistrationsAsync();
        public Task<int> GetTotalEventsAsync() => dashboardRepository.GetTotalEventsAsync();
        public Task<int> GetTotalUsersAsync() => dashboardRepository.GetTotalUsersAsync();
        public Task<int> GetTotalOrganizersAsync() => dashboardRepository.GetTotalOrganizersAsync();
        public Task<MostRegisteredEventDto> GetMostRegisteredEventAsync() => dashboardRepository.GetMostRegisteredEventAsync();
        public Task<Dictionary<string, int>> GetRegistrationsPerMonthAsync() => dashboardRepository.GetRegistrationsPerMonthAsync();
    }
}
