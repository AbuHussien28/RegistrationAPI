using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RegistrationAPI.Infrastructure.Services;
using RegistrationAPI.Shared.DTOS;

namespace RegistrationAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDTO model)
        {
            if (!ModelState.IsValid)
            {
                var firstError = ModelState.Values.SelectMany(v => v.Errors)
                                                  .Select(e => e.ErrorMessage)
                                                  .FirstOrDefault();
                return BadRequest(new { message = firstError ?? "Validation failed" });
            }
            var result = await authService.RegisterAsync(model);
            if (result.StartsWith("error"))
                return BadRequest(result);
            return Ok(new { Token = result });
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO model)
        {
            var result = await authService.LoginAsync(model);
            if (result.Contains("error") || result.Contains("failed"))
                return BadRequest(result);
            return Ok(new { Token = result });
        }

    }
}
