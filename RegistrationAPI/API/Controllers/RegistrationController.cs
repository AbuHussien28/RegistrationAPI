using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RegistrationAPI.Core.Interfaces;
using RegistrationAPI.Infrastructure.Services;
using RegistrationAPI.Shared.DTOS.Registration;
using System.Security.Claims;

namespace RegistrationAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly IRegistrationService registrationService;

        public RegistrationController(IRegistrationService registrationService)
        {
            this.registrationService = registrationService;
        }
        [HttpPost("BookEvent")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> RegisterToEvent([FromBody] RegistrationCreateDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await registrationService.RegisterToEventAsync(dto);
            return Ok(new { message = result });
        }
        [HttpGet("eventRegistration/{eventId}")]
        [Authorize(Roles = "Admin,Organizer")]
        public async Task<IActionResult> GetRegistrationsForEvent(int eventId)
        {
            var result = await registrationService.GetRegistrationsForEventAsync(eventId);
            if (result == null || !result.Any())
                return NotFound(new { message = "No registrations found for this event." });
            return Ok(result);
        }
        [HttpGet("userRegistration/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserRegistrations(string userId)
        {
            var result = await registrationService.GetUserRegistrationsAsync(userId);

            if (result == null || !result.Any())
                return NotFound(new { message = "No registrations found for this user." });

            return Ok(result);
        }
        [HttpGet("myRegistrations")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetMyRegistrations()
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(currentUserId))
                return Unauthorized(new { message = "User ID not found in token." });

            var result = await registrationService.GetUserRegistrationsAsync(currentUserId);

            if (result == null || !result.Any())
                return NotFound(new { message = "No registrations found for the current user." });

            return Ok(result);
        }
    }
}
