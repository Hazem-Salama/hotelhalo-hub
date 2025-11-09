using HotelHaloHub.API.Models.Entities;

namespace HotelHaloHub.API.Repositories.Interfaces;

public interface IBookingRepository : IRepository<Booking>
{
    Task<IEnumerable<Booking>> GetByHotelIdAsync(string hotelId);
    Task<IEnumerable<Booking>> GetBookingsByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<IEnumerable<Booking>> GetTodayBookingsAsync();
    Task<IEnumerable<Booking>> GetRecentBookingsAsync(int limit = 10);
}
