using RegistrationAPI.Shared.DTOS.Registration;

namespace RegistrationAPI.Core.Interfaces
{
    public interface IRegistrationService
    {
        Task<string> RegisterToEventAsync(RegistrationCreateDTO dto);
        Task<List<RegistrationDTO>> GetRegistrationsForEventAsync(int eventId);
        Task<List<RegistrationDTO>> GetUserRegistrationsAsync(string userId);
    }
}
