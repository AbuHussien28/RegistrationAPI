using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using RegistrationAPI.Core.Interfaces;
using RegistrationAPI.Core.Models;
using RegistrationAPI.Shared.DTOS;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RegistrationAPI.Infrastructure.Services
{
    public class AuthService:IAuthService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IConfiguration config;
        private readonly IMapper mapper;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IEmailSender emailSender;

        public AuthService(UserManager<ApplicationUser> userManager,
                        SignInManager<ApplicationUser> signInManager,
                         IConfiguration config,IMapper mapper,
                         RoleManager<IdentityRole> roleManager,
                         IEmailSender emailSender

           )
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.config = config;
            this.mapper = mapper;
            this.roleManager = roleManager;
            this.emailSender = emailSender;
        }
        public async Task<string> RegisterAsync(RegisterDTO model)
        {
            var user = new ApplicationUser
            {
                FullName = model.FullName,
                UserName = model.UserName,
                Email = model.Email
            };

            var result = await userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return "error: " + string.Join(", ", result.Errors.Select(e => e.Description));

            await userManager.AddToRoleAsync(user, model.Role);
            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = $"http://localhost:8989/api/auth/confirm-email?userId={user.Id}&token={Uri.EscapeDataString(token)}";
            await emailSender.SendEmailAsync(user.Email, "Confirm your account", $"Click <a href='{confirmationLink}'>here</a> to confirm your account.");
            return "Registration successful. Please check your email to confirm your account.";
        }

        public async Task<string> LoginAsync(LoginDTO model)
        {
            var user = await userManager.FindByNameAsync(model.UserName);

            if (user == null )
                return "error: Username or password is incorrect";
            if (!user.IsActive)
                return "error: Username is InActive";
            if (!user.EmailConfirmed)
                return "error: Please confirm your email before logging in.";
            var result = await signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (!result.Succeeded)
                return "error: Username or password is incorrect";
            if (!string.IsNullOrEmpty(user.CurrentToken) && user.TokenExpiration.HasValue && user.TokenExpiration > DateTime.UtcNow)
            {
                return user.CurrentToken;
            }

            var newToken = GenerateToken(user);
            user.CurrentToken = newToken;
            user.TokenExpiration = DateTime.UtcNow.AddDays(7);
            await userManager.UpdateAsync(user);

            return newToken;
        }
        private string GenerateToken(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName)
            };
            var roles = userManager.GetRolesAsync(user).Result;
            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public async Task<string> ConfirmEmailAsync(string userId, string token)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
                return "error: Invalid user ID.";

            var result = await userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
                return "Email confirmed successfully.";
            return "error: Email confirmation failed.";
        }
        public async Task<bool?> IsEmailConfirmedAsync(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
                return null;

            return user.EmailConfirmed;
        }
        public async Task<bool> ResendEmailConfirmationAsync(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null || user.EmailConfirmed)
                return false;

            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = $"http://localhost:8989/confirm-email?userId={user.Id}&token={Uri.EscapeDataString(token)}";
          await emailSender.SendEmailAsync(user.Email, "Confirm your email", $"Click this link: {confirmationLink}");
            return true;
        }

    }
}
