using HotelHaloHub.API.Models.DTOs;

namespace HotelHaloHub.API.Services.Interfaces;

public interface IBookingService
{
    Task<IEnumerable<BookingDto>> GetAllBookingsAsync();
    Task<BookingDto?> GetBookingByIdAsync(string id);
    Task<BookingDto> CreateBookingAsync(CreateBookingDto createBookingDto, string hotelId);
    Task<bool> UpdateBookingStatusAsync(string id, string status);
    Task<bool> DeleteBookingAsync(string id);
}
