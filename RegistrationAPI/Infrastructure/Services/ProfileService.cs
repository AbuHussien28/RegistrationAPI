using Microsoft.AspNetCore.Identity;
using RegistrationAPI.Core.Interfaces;
using RegistrationAPI.Core.Models;
using RegistrationAPI.Shared.DTOS.Profiles;
using System.Security.Claims;

namespace RegistrationAPI.Infrastructure.Services
{
    public class ProfileService : IProfileService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IHttpContextAccessor httpContextAccessor;

        public ProfileService(UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            this.userManager = userManager;
            this.httpContextAccessor = httpContextAccessor;
        }
        private async Task<ApplicationUser> GetCurrentUserAsync()
        {
            var userId = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                throw new UnauthorizedAccessException("User not logged in.");

            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
                throw new Exception("User not found.");

            return user;
        }

        public async Task<string> UpdateUserNameAsync(UpdateUserNameDTO model)
        {
            var user = await GetCurrentUserAsync();

            if (!string.IsNullOrEmpty(model.NewUserName) && user.UserName != model.NewUserName)
            {
                user.UserName = model.NewUserName;
                user.NormalizedUserName = model.NewUserName.ToUpper();

                var updateResult = await userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                    throw new Exception(string.Join(", ", updateResult.Errors.Select(e => e.Description)));

                return "Username updated successfully.";
            }

            return "No changes made to username.";
        }

        public async Task<string> UpdatePasswordAsync(UpdatePasswordDTO model)
        {
            var user = await GetCurrentUserAsync();

            var isPasswordCorrect = await userManager.CheckPasswordAsync(user, model.OldPassword);
            if (!isPasswordCorrect)
                throw new Exception("Old password is incorrect.");

            if (model.NewPassword != model.ConfirmPassword)
                throw new Exception("New password and confirmation do not match.");

            var passwordChangeResult = await userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (!passwordChangeResult.Succeeded)
                throw new Exception(string.Join(", ", passwordChangeResult.Errors.Select(e => e.Description)));

            return "Password updated successfully.";
        }
        public async Task<string> GetCurrentUserNameAsync()
        {
            var user = await GetCurrentUserAsync();
            return user.UserName ?? string.Empty;
        }

    }
}
