using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RegistrationAPI.Core.Interfaces;
using RegistrationAPI.Shared.DTOS.Events;
using System.Security.Claims;

namespace RegistrationAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventService eventService;

        public EventsController(IEventService eventService)
        {
            this.eventService = eventService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await eventService.GetAllAsync();
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await eventService.GetByIdAsync(id);
            if (result == null)
                return NotFound("Event not found.");
            if (result.IsDeleted)
                return NotFound("This event is deleted.");
            return Ok(result);
        }
        [Authorize(Roles = "Admin,Organizer")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EventCreateDto dto)
        {
            if (dto == null)
            {
                return BadRequest(new { message = "Request body is required and must be a valid JSON object." });
            }
            if (!ModelState.IsValid)
            {
                var firstError = ModelState.Values.SelectMany(v => v.Errors)
                                                  .Select(e => e.ErrorMessage)
                                                  .FirstOrDefault();
                return BadRequest(new { message = firstError ?? "Validation failed" });
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var createdEvent = await eventService.CreateAsync(dto, userId);
            return Ok(createdEvent);
        }
        [Authorize(Roles = "Admin,Organizer")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] EventCreateDto dto)
        {
            if (dto == null)
            {
                return BadRequest(new { message = "Request body is required and must be a valid JSON object." });
            }
            if (!ModelState.IsValid)
            {
                var firstError = ModelState.Values.SelectMany(v => v.Errors)
                                                  .Select(e => e.ErrorMessage)
                                                  .FirstOrDefault();
                return BadRequest(new { message = firstError ?? "Validation failed" });
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var role = User.IsInRole("Admin") ? "Admin" : "Organizer";

            var success = await eventService.UpdateAsync(id, dto, userId, role);
            if (!success) return Forbid();

            return Ok("Updated successfully");
        }
        [Authorize(Roles = "Admin,Organizer")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var role = User.IsInRole("Admin") ? "Admin" : "Organizer";

            var success = await eventService.DeleteAsync(id, userId, role);
            if (!success) return Forbid();

            return Ok("Deleted (soft delete)");
        }
        [Authorize(Roles = "Admin,Organizer")]
        [HttpPut("restore/{id}")]
        public async Task<IActionResult> Restore(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var role = User.IsInRole("Admin") ? "Admin" : "Organizer";

            var success = await eventService.RestoreAsync(id, userId, role);
            if (!success) return Forbid();

            return Ok("Event restored successfully");
        }
        [Authorize(Roles = "Organizer")]
        [HttpGet("MyEvents")]
        public async Task<IActionResult> GetMyEvents()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var events = await eventService.GetMyEventsAsync(userId);
            return Ok(events);
        }
    }
}
