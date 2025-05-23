using RegistrationAPI.Core.Models;

namespace RegistrationAPI.Shared.DTOS
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(RegisterDTO model);
        Task<string> LoginAsync(LoginDTO model);
       


    }
}
