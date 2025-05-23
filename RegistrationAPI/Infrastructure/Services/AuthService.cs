using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
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

        public AuthService(UserManager<ApplicationUser> userManager,
                        SignInManager<ApplicationUser> signInManager,
                         IConfiguration config,IMapper mapper,
                         RoleManager<IdentityRole> roleManager

           )
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.config = config;
            this.mapper = mapper;
            this.roleManager = roleManager;
        }
        public async Task<string> RegisterAsync(RegisterDTO model)
        {
            var roleExists = await roleManager.RoleExistsAsync(model.Role);
            if (!roleExists)
                return "error: Role not found.";
            var user = mapper.Map<ApplicationUser>(model);
            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return "error: " + string.Join("; ", result.Errors.Select(e => e.Description));
            await userManager.AddToRoleAsync(user, model.Role);
            var token = GenerateToken(user);
            user.CurrentToken = token;
            user.TokenExpiration = DateTime.UtcNow.AddDays(7);
            await userManager.UpdateAsync(user);

            return token;
        }
        public async Task<string> LoginAsync(LoginDTO model)
        {
            var user = await userManager.FindByNameAsync(model.UserName);

            if (user == null )
                return "error: Username or password is incorrect";
            if (!user.IsActive)
                return "error: Username is InActive";
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
    }
}
