using HotelHaloHub.API.Configuration;
using HotelHaloHub.API.Models.Entities;
using HotelHaloHub.API.Repositories.Interfaces;

namespace HotelHaloHub.API.Repositories.Implementations;

public class InMemoryBookingRepository : InMemoryRepository<Booking>, IBookingRepository
{
    public InMemoryBookingRepository()
        : base(InMemoryDataStore.Instance.Bookings, b => b.Id)
    {
    }

    public Task<IEnumerable<Booking>> GetByHotelIdAsync(string hotelId)
    {
        var bookings = _collection
            .Where(b => b.HotelId == hotelId)
            .OrderByDescending(b => b.CreatedAt)
            .ToList();
        return Task.FromResult<IEnumerable<Booking>>(bookings);
    }

    public Task<IEnumerable<Booking>> GetBookingsByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        var bookings = _collection
            .Where(b => b.CheckInDate >= startDate && b.CheckOutDate <= endDate)
            .ToList();
        return Task.FromResult<IEnumerable<Booking>>(bookings);
    }

    public Task<IEnumerable<Booking>> GetTodayBookingsAsync()
    {
        var today = DateTime.UtcNow.Date;
        var tomorrow = today.AddDays(1);

        var bookings = _collection
            .Where(b =>
                (b.CheckInDate >= today && b.CheckInDate < tomorrow) ||
                (b.CheckOutDate >= today && b.CheckOutDate < tomorrow))
            .ToList();

        return Task.FromResult<IEnumerable<Booking>>(bookings);
    }

    public Task<IEnumerable<Booking>> GetRecentBookingsAsync(int limit = 10)
    {
        var bookings = _collection
            .OrderByDescending(b => b.CreatedAt)
            .Take(limit)
            .ToList();
        return Task.FromResult<IEnumerable<Booking>>(bookings);
    }
}
