using Microsoft.AspNetCore.Mvc;
using HotelHaloHub.API.Models.DTOs;
using HotelHaloHub.API.Services.Interfaces;

namespace HotelHaloHub.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HotelController : ControllerBase
{
    private readonly IHotelService _hotelService;
    private readonly ILogger<HotelController> _logger;

    public HotelController(IHotelService hotelService, ILogger<HotelController> logger)
    {
        _hotelService = hotelService;
        _logger = logger;
    }

    [HttpGet("settings")]
    public async Task<ActionResult<HotelDto>> GetHotelSettings()
    {
        try
        {
            var hotel = await _hotelService.GetHotelSettingsAsync();
            if (hotel == null)
            {
                // Create default hotel if none exists
                var hotelId = await _hotelService.GetOrCreateDefaultHotelIdAsync();
                hotel = await _hotelService.GetHotelSettingsAsync();
            }

            return Ok(hotel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting hotel settings");
            return StatusCode(500, new { message = "An error occurred while fetching hotel settings" });
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<HotelDto>> UpdateHotelSettings(string id, [FromBody] UpdateHotelDto updateHotelDto)
    {
        try
        {
            var hotel = await _hotelService.UpdateHotelSettingsAsync(id, updateHotelDto);
            return Ok(hotel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating hotel settings");
            return StatusCode(500, new { message = ex.Message });
        }
    }
}
