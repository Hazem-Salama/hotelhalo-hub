using MongoDB.Driver;
using HotelHaloHub.API.Models.Entities;
using HotelHaloHub.API.Repositories.Interfaces;
using HotelHaloHub.API.Configuration;
using Microsoft.Extensions.Options;

namespace HotelHaloHub.API.Repositories.Implementations;

public class HotelRepository : MongoRepository<Hotel>, IHotelRepository
{
    public HotelRepository(IMongoDatabase database, IOptions<MongoDbSettings> settings)
        : base(database, settings.Value.HotelsCollectionName)
    {
    }

    public async Task<Hotel?> GetFirstAsync()
    {
        return await _collection.Find(_ => true).FirstOrDefaultAsync();
    }
}
