using Microsoft.AspNetCore.Mvc;
using HotelHaloHub.API.Models.DTOs;
using HotelHaloHub.API.Services.Interfaces;

namespace HotelHaloHub.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingsController : ControllerBase
{
    private readonly IBookingService _bookingService;
    private readonly IHotelService _hotelService;
    private readonly ILogger<BookingsController> _logger;

    public BookingsController(IBookingService bookingService, IHotelService hotelService, ILogger<BookingsController> logger)
    {
        _bookingService = bookingService;
        _hotelService = hotelService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookingDto>>> GetAllBookings()
    {
        try
        {
            var bookings = await _bookingService.GetAllBookingsAsync();
            return Ok(bookings);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all bookings");
            return StatusCode(500, new { message = "An error occurred while fetching bookings" });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BookingDto>> GetBookingById(string id)
    {
        try
        {
            var booking = await _bookingService.GetBookingByIdAsync(id);
            if (booking == null)
                return NotFound(new { message = $"Booking with ID {id} not found" });

            return Ok(booking);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting booking with ID {BookingId}", id);
            return StatusCode(500, new { message = "An error occurred while fetching the booking" });
        }
    }

    [HttpPost]
    public async Task<ActionResult<BookingDto>> CreateBooking([FromBody] CreateBookingDto createBookingDto)
    {
        try
        {
            var hotelId = await _hotelService.GetOrCreateDefaultHotelIdAsync();
            var booking = await _bookingService.CreateBookingAsync(createBookingDto, hotelId);
            return CreatedAtAction(nameof(GetBookingById), new { id = booking.Id }, booking);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating booking");
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpPatch("{id}/status")]
    public async Task<IActionResult> UpdateBookingStatus(string id, [FromBody] UpdateBookingStatusDto updateStatusDto)
    {
        try
        {
            var updated = await _bookingService.UpdateBookingStatusAsync(id, updateStatusDto.Status);
            if (!updated)
                return NotFound(new { message = $"Booking with ID {id} not found" });

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating booking status for ID {BookingId}", id);
            return StatusCode(500, new { message = "An error occurred while updating the booking status" });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBooking(string id)
    {
        try
        {
            var deleted = await _bookingService.DeleteBookingAsync(id);
            if (!deleted)
                return NotFound(new { message = $"Booking with ID {id} not found" });

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting booking with ID {BookingId}", id);
            return StatusCode(500, new { message = "An error occurred while deleting the booking" });
        }
    }
}
