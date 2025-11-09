using MongoDB.Driver;
using HotelHaloHub.API.Models.Entities;
using HotelHaloHub.API.Repositories.Interfaces;
using HotelHaloHub.API.Configuration;
using Microsoft.Extensions.Options;

namespace HotelHaloHub.API.Repositories.Implementations;

public class RoomRepository : MongoRepository<Room>, IRoomRepository
{
    public RoomRepository(IMongoDatabase database, IOptions<MongoDbSettings> settings)
        : base(database, settings.Value.RoomsCollectionName)
    {
    }

    public async Task<Room?> GetByNumberAsync(string roomNumber)
    {
        var filter = Builders<Room>.Filter.Eq(r => r.Number, roomNumber);
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Room>> GetByHotelIdAsync(string hotelId)
    {
        var filter = Builders<Room>.Filter.Eq(r => r.HotelId, hotelId);
        return await _collection.Find(filter).ToListAsync();
    }

    public async Task<IEnumerable<Room>> GetAvailableRoomsAsync(string hotelId)
    {
        var filter = Builders<Room>.Filter.And(
            Builders<Room>.Filter.Eq(r => r.HotelId, hotelId),
            Builders<Room>.Filter.Eq(r => r.Status, RoomStatus.Available)
        );
        return await _collection.Find(filter).ToListAsync();
    }
}
