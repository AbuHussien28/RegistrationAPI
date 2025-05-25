using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RegistrationAPI.Core.Interfaces;
using RegistrationAPI.Infrastructure.Services;

namespace RegistrationAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            this.dashboardService = dashboardService;
        }
        [HttpGet("total-registrations")]
        public async Task<IActionResult> GetTotalRegistrations()
        {
            var result = await dashboardService.GetTotalRegistrationsAsync();
            return Ok(result);
        }

        [HttpGet("total-events")]
        public async Task<IActionResult> GetTotalEvents()
        {
            var result = await dashboardService.GetTotalEventsAsync();
            return Ok(result);
        }

        [HttpGet("total-users")]
        public async Task<IActionResult> GetTotalUsers()
        {
            var result = await dashboardService.GetTotalUsersAsync();
            return Ok(result);
        }

        [HttpGet("total-organizers")]
        public async Task<IActionResult> GetTotalOrganizers()
        {
            var result = await dashboardService.GetTotalOrganizersAsync();
            return Ok(result);
        }

        [HttpGet("most-registered-event")]
        public async Task<IActionResult> GetMostRegisteredEvent()
        {
            var result = await dashboardService.GetMostRegisteredEventAsync();
            return Ok(result); 
        }
        [HttpGet("registrations-per-month")]
        public async Task<IActionResult> GetRegistrationsPerMonth()
        {
            var result = await dashboardService.GetRegistrationsPerMonthAsync();
            return Ok(result);
        }
    }
}
