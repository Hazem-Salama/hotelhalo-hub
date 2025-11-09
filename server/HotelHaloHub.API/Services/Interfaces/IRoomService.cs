using HotelHaloHub.API.Models.DTOs;

namespace HotelHaloHub.API.Services.Interfaces;

public interface IRoomService
{
    Task<IEnumerable<RoomDto>> GetAllRoomsAsync();
    Task<RoomDto?> GetRoomByIdAsync(string id);
    Task<RoomDto> CreateRoomAsync(CreateRoomDto createRoomDto, string hotelId);
    Task<bool> UpdateRoomAsync(string id, UpdateRoomDto updateRoomDto);
    Task<bool> DeleteRoomAsync(string id);
}
