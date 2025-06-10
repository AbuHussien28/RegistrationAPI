using RegistrationAPI.Core.Models;

namespace RegistrationAPI.Shared.DTOS
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(RegisterDTO model);
        Task<string> LoginAsync(LoginDTO model);
        Task<string> ConfirmEmailAsync(string userId, string token);
        Task<bool?> IsEmailConfirmedAsync(string userId);
        Task<bool> ResendEmailConfirmationAsync(string userId);


    }
}
