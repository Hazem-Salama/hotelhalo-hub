using HotelHaloHub.API.Configuration;
using HotelHaloHub.API.Models.Entities;
using HotelHaloHub.API.Repositories.Interfaces;

namespace HotelHaloHub.API.Repositories.Implementations;

public class InMemoryHotelRepository : InMemoryRepository<Hotel>, IHotelRepository
{
    public InMemoryHotelRepository()
        : base(InMemoryDataStore.Instance.Hotels, h => h.Id)
    {
    }

    public Task<Hotel?> GetFirstAsync()
    {
        var hotel = _collection.FirstOrDefault();
        return Task.FromResult(hotel);
    }
}
