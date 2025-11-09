using MongoDB.Driver;
using HotelHaloHub.API.Models.Entities;
using HotelHaloHub.API.Repositories.Interfaces;
using HotelHaloHub.API.Configuration;
using Microsoft.Extensions.Options;

namespace HotelHaloHub.API.Repositories.Implementations;

public class BookingRepository : MongoRepository<Booking>, IBookingRepository
{
    public BookingRepository(IMongoDatabase database, IOptions<MongoDbSettings> settings)
        : base(database, settings.Value.BookingsCollectionName)
    {
    }

    public async Task<IEnumerable<Booking>> GetByHotelIdAsync(string hotelId)
    {
        var filter = Builders<Booking>.Filter.Eq(b => b.HotelId, hotelId);
        return await _collection.Find(filter)
            .SortByDescending(b => b.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Booking>> GetBookingsByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        var filter = Builders<Booking>.Filter.And(
            Builders<Booking>.Filter.Gte(b => b.CheckInDate, startDate),
            Builders<Booking>.Filter.Lte(b => b.CheckOutDate, endDate)
        );
        return await _collection.Find(filter).ToListAsync();
    }

    public async Task<IEnumerable<Booking>> GetTodayBookingsAsync()
    {
        var today = DateTime.UtcNow.Date;
        var tomorrow = today.AddDays(1);

        var filter = Builders<Booking>.Filter.Or(
            Builders<Booking>.Filter.And(
                Builders<Booking>.Filter.Gte(b => b.CheckInDate, today),
                Builders<Booking>.Filter.Lt(b => b.CheckInDate, tomorrow)
            ),
            Builders<Booking>.Filter.And(
                Builders<Booking>.Filter.Gte(b => b.CheckOutDate, today),
                Builders<Booking>.Filter.Lt(b => b.CheckOutDate, tomorrow)
            )
        );

        return await _collection.Find(filter).ToListAsync();
    }

    public async Task<IEnumerable<Booking>> GetRecentBookingsAsync(int limit = 10)
    {
        return await _collection.Find(_ => true)
            .SortByDescending(b => b.CreatedAt)
            .Limit(limit)
            .ToListAsync();
    }
}
