using HotelHaloHub.API.Models.Entities;

namespace HotelHaloHub.API.Repositories.Interfaces;

public interface IRoomRepository : IRepository<Room>
{
    Task<Room?> GetByNumberAsync(string roomNumber);
    Task<IEnumerable<Room>> GetByHotelIdAsync(string hotelId);
    Task<IEnumerable<Room>> GetAvailableRoomsAsync(string hotelId);
}
