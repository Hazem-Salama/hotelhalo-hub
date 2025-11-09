using HotelHaloHub.API.Configuration;
using HotelHaloHub.API.Models.Entities;
using HotelHaloHub.API.Repositories.Interfaces;

namespace HotelHaloHub.API.Repositories.Implementations;

public class InMemoryRoomRepository : InMemoryRepository<Room>, IRoomRepository
{
    public InMemoryRoomRepository()
        : base(InMemoryDataStore.Instance.Rooms, r => r.Id)
    {
    }

    public Task<Room?> GetByNumberAsync(string roomNumber)
    {
        var room = _collection.FirstOrDefault(r => r.Number == roomNumber);
        return Task.FromResult(room);
    }

    public Task<IEnumerable<Room>> GetByHotelIdAsync(string hotelId)
    {
        var rooms = _collection.Where(r => r.HotelId == hotelId).ToList();
        return Task.FromResult<IEnumerable<Room>>(rooms);
    }

    public Task<IEnumerable<Room>> GetAvailableRoomsAsync(string hotelId)
    {
        var rooms = _collection.Where(r => r.HotelId == hotelId && r.Status == RoomStatus.Available).ToList();
        return Task.FromResult<IEnumerable<Room>>(rooms);
    }
}
