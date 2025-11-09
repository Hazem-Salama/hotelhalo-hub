using HotelHaloHub.API.Models.Entities;

namespace HotelHaloHub.API.Repositories.Interfaces;

public interface IHotelRepository : IRepository<Hotel>
{
    Task<Hotel?> GetFirstAsync();
}
