using RegistrationAPI.Shared.DTOS.Profiles;

namespace RegistrationAPI.Core.Interfaces
{
    public interface IProfileService
    {
        Task<string> UpdateUserNameAsync(UpdateUserNameDTO model);
        Task<string> UpdatePasswordAsync(UpdatePasswordDTO model);
    }
}
