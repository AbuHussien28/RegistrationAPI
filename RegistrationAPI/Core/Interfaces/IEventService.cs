using RegistrationAPI.Shared.DTOS.Events;

namespace RegistrationAPI.Core.Interfaces
{
    public interface IEventService
    {
        Task<List<EventReadDto>> GetAllAsync();
        Task<EventReadDto> GetByIdAsync(int id);
        Task<EventReadDto> CreateAsync(EventCreateDto dto, string userId);
        Task<bool> UpdateAsync(int id, EventCreateDto dto, string userId, string role);
        Task<bool> DeleteAsync(int id, string userId, string role);
        Task<bool> RestoreAsync(int id, string userId, string role);
        Task<List<EventReadDto>> GetMyEventsAsync(string userId);
        Task<List<EventReadDto>> GetAllDeletedEventsAsync();

    }
}
