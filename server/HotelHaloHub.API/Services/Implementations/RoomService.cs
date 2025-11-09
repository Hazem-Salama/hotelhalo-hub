using AutoMapper;
using HotelHaloHub.API.Models.DTOs;
using HotelHaloHub.API.Models.Entities;
using HotelHaloHub.API.Repositories.Interfaces;
using HotelHaloHub.API.Services.Interfaces;

namespace HotelHaloHub.API.Services.Implementations;

public class RoomService : IRoomService
{
    private readonly IRoomRepository _roomRepository;
    private readonly IMapper _mapper;

    public RoomService(IRoomRepository roomRepository, IMapper mapper)
    {
        _roomRepository = roomRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<RoomDto>> GetAllRoomsAsync()
    {
        var rooms = await _roomRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<RoomDto>>(rooms);
    }

    public async Task<RoomDto?> GetRoomByIdAsync(string id)
    {
        var room = await _roomRepository.GetByIdAsync(id);
        return room == null ? null : _mapper.Map<RoomDto>(room);
    }

    public async Task<RoomDto> CreateRoomAsync(CreateRoomDto createRoomDto, string hotelId)
    {
        var room = _mapper.Map<Room>(createRoomDto);
        room.HotelId = hotelId;
        room.CreatedAt = DateTime.UtcNow;
        room.UpdatedAt = DateTime.UtcNow;

        var createdRoom = await _roomRepository.CreateAsync(room);
        return _mapper.Map<RoomDto>(createdRoom);
    }

    public async Task<bool> UpdateRoomAsync(string id, UpdateRoomDto updateRoomDto)
    {
        var existingRoom = await _roomRepository.GetByIdAsync(id);
        if (existingRoom == null)
            return false;

        // Update only provided fields
        if (!string.IsNullOrEmpty(updateRoomDto.Number))
            existingRoom.Number = updateRoomDto.Number;

        if (!string.IsNullOrEmpty(updateRoomDto.Type))
            existingRoom.Type = updateRoomDto.Type;

        if (!string.IsNullOrEmpty(updateRoomDto.Status))
            existingRoom.Status = ParseRoomStatus(updateRoomDto.Status);

        if (updateRoomDto.Price.HasValue)
            existingRoom.Price = updateRoomDto.Price.Value;

        if (updateRoomDto.Capacity.HasValue)
            existingRoom.Capacity = updateRoomDto.Capacity.Value;

        existingRoom.UpdatedAt = DateTime.UtcNow;

        return await _roomRepository.UpdateAsync(id, existingRoom);
    }

    public async Task<bool> DeleteRoomAsync(string id)
    {
        return await _roomRepository.DeleteAsync(id);
    }

    private static RoomStatus ParseRoomStatus(string status)
    {
        return status.ToLower() switch
        {
            "available" => RoomStatus.Available,
            "occupied" => RoomStatus.Occupied,
            "maintenance" => RoomStatus.Maintenance,
            _ => RoomStatus.Available
        };
    }
}
