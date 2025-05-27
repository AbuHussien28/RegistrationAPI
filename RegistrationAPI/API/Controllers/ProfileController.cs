using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RegistrationAPI.Core.Interfaces;
using RegistrationAPI.Infrastructure.Services;
using RegistrationAPI.Shared.DTOS.Profiles;
using System.Security.Claims;

namespace RegistrationAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;

        public ProfileController(IProfileService service)
        {
            _profileService = service;
        }
        [HttpPut("update-username")]
        public async Task<IActionResult> UpdateUserName([FromBody] UpdateUserNameDTO model)
        {
            try
            {
                var result = await _profileService.UpdateUserNameAsync(model);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update-password")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordDTO model)
        {
            try
            {
                var result = await _profileService.UpdatePasswordAsync(model);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("current-username")]
        public async Task<IActionResult> GetCurrentUserName()
        {
            try
            {
                var username = await _profileService.GetCurrentUserNameAsync();
                return Ok(new { userName = username });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
