using RegistrationAPI.Core.Models;

namespace RegistrationAPI.Core.Interfaces
{
    public interface IRegistrationRepository
    {
        Task<Registration> GetByUserAndEventAsync(string userId, int eventId);
        Task<List<Registration>> GetAllByEventIdAsync(int eventId);
        Task<List<Registration>> GetAllByUserIdAsync(string userId);
    }
}
