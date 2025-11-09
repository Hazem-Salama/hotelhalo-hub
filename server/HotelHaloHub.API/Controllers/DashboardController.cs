using Microsoft.AspNetCore.Mvc;
using HotelHaloHub.API.Models.DTOs;
using HotelHaloHub.API.Services.Interfaces;

namespace HotelHaloHub.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _dashboardService;
    private readonly IHotelService _hotelService;
    private readonly ILogger<DashboardController> _logger;

    public DashboardController(IDashboardService dashboardService, IHotelService hotelService, ILogger<DashboardController> logger)
    {
        _dashboardService = dashboardService;
        _hotelService = hotelService;
        _logger = logger;
    }

    [HttpGet("stats")]
    public async Task<ActionResult<DashboardStatsDto>> GetDashboardStats()
    {
        try
        {
            var hotelId = await _hotelService.GetOrCreateDefaultHotelIdAsync();
            var stats = await _dashboardService.GetDashboardStatsAsync(hotelId);
            return Ok(stats);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting dashboard stats");
            return StatusCode(500, new { message = "An error occurred while fetching dashboard statistics" });
        }
    }
}
