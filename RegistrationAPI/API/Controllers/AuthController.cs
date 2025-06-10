using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RegistrationAPI.Infrastructure.Services;
using RegistrationAPI.Shared.DTOS;
using System.Security.Claims;

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
        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var result = await authService.ConfirmEmailAsync(userId, token);

            if (result.StartsWith("error"))
                return BadRequest(result);

            return Ok(result);
        }
        [HttpGet("is-email-confirmed")]
        public async Task<IActionResult> IsEmailConfirmed()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return Unauthorized();

            var confirmed = await authService.IsEmailConfirmedAsync(userId);

            if (confirmed == null)
                return NotFound();

            return Ok(new { confirmed });
        }
        [HttpPost("resend-confirmation")]
        public async Task<IActionResult> ResendConfirmation()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized();

            var success = await authService.ResendEmailConfirmationAsync(userId);
            if (!success)
                return BadRequest("User not found or already confirmed.");

            return Ok("Confirmation email resent.");
        }

    }
}
