using Microsoft.AspNetCore.Mvc;
using HotelHaloHub.API.Models.DTOs;
using HotelHaloHub.API.Services.Interfaces;

namespace HotelHaloHub.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomsController : ControllerBase
{
    private readonly IRoomService _roomService;
    private readonly IHotelService _hotelService;
    private readonly ILogger<RoomsController> _logger;

    public RoomsController(IRoomService roomService, IHotelService hotelService, ILogger<RoomsController> logger)
    {
        _roomService = roomService;
        _hotelService = hotelService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RoomDto>>> GetAllRooms()
    {
        try
        {
            var rooms = await _roomService.GetAllRoomsAsync();
            return Ok(rooms);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all rooms");
            return StatusCode(500, new { message = "An error occurred while fetching rooms" });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RoomDto>> GetRoomById(string id)
    {
        try
        {
            var room = await _roomService.GetRoomByIdAsync(id);
            if (room == null)
                return NotFound(new { message = $"Room with ID {id} not found" });

            return Ok(room);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting room with ID {RoomId}", id);
            return StatusCode(500, new { message = "An error occurred while fetching the room" });
        }
    }

    [HttpPost]
    public async Task<ActionResult<RoomDto>> CreateRoom([FromBody] CreateRoomDto createRoomDto)
    {
        try
        {
            var hotelId = await _hotelService.GetOrCreateDefaultHotelIdAsync();
            var room = await _roomService.CreateRoomAsync(createRoomDto, hotelId);
            return CreatedAtAction(nameof(GetRoomById), new { id = room.Id }, room);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating room");
            return StatusCode(500, new { message = "An error occurred while creating the room" });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRoom(string id, [FromBody] UpdateRoomDto updateRoomDto)
    {
        try
        {
            var updated = await _roomService.UpdateRoomAsync(id, updateRoomDto);
            if (!updated)
                return NotFound(new { message = $"Room with ID {id} not found" });

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating room with ID {RoomId}", id);
            return StatusCode(500, new { message = "An error occurred while updating the room" });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRoom(string id)
    {
        try
        {
            var deleted = await _roomService.DeleteRoomAsync(id);
            if (!deleted)
                return NotFound(new { message = $"Room with ID {id} not found" });

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting room with ID {RoomId}", id);
            return StatusCode(500, new { message = "An error occurred while deleting the room" });
        }
    }
}
