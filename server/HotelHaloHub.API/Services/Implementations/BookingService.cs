using AutoMapper;
using HotelHaloHub.API.Models.DTOs;
using HotelHaloHub.API.Models.Entities;
using HotelHaloHub.API.Repositories.Interfaces;
using HotelHaloHub.API.Services.Interfaces;

namespace HotelHaloHub.API.Services.Implementations;

public class BookingService : IBookingService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IMapper _mapper;

    public BookingService(IBookingRepository bookingRepository, IRoomRepository roomRepository, IMapper mapper)
    {
        _bookingRepository = bookingRepository;
        _roomRepository = roomRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<BookingDto>> GetAllBookingsAsync()
    {
        var bookings = await _bookingRepository.GetAllAsync();
        var bookingDtos = new List<BookingDto>();

        foreach (var booking in bookings)
        {
            var bookingDto = _mapper.Map<BookingDto>(booking);

            // Get room number for the booking
            var room = await _roomRepository.GetByIdAsync(booking.RoomId);
            bookingDto.RoomNumber = room?.Number ?? "Unknown";

            bookingDtos.Add(bookingDto);
        }

        return bookingDtos;
    }

    public async Task<BookingDto?> GetBookingByIdAsync(string id)
    {
        var booking = await _bookingRepository.GetByIdAsync(id);
        if (booking == null)
            return null;

        var bookingDto = _mapper.Map<BookingDto>(booking);

        // Get room number
        var room = await _roomRepository.GetByIdAsync(booking.RoomId);
        bookingDto.RoomNumber = room?.Number ?? "Unknown";

        return bookingDto;
    }

    public async Task<BookingDto> CreateBookingAsync(CreateBookingDto createBookingDto, string hotelId)
    {
        // Find room by room number
        var room = await _roomRepository.GetByNumberAsync(createBookingDto.RoomNumber);
        if (room == null)
            throw new Exception($"Room {createBookingDto.RoomNumber} not found");

        var booking = _mapper.Map<Booking>(createBookingDto);
        booking.HotelId = hotelId;
        booking.RoomId = room.Id;
        booking.Status = BookingStatus.Reserved;
        booking.CreatedAt = DateTime.UtcNow;
        booking.UpdatedAt = DateTime.UtcNow;

        var createdBooking = await _bookingRepository.CreateAsync(booking);

        var bookingDto = _mapper.Map<BookingDto>(createdBooking);
        bookingDto.RoomNumber = room.Number;

        return bookingDto;
    }

    public async Task<bool> UpdateBookingStatusAsync(string id, string status)
    {
        var existingBooking = await _bookingRepository.GetByIdAsync(id);
        if (existingBooking == null)
            return false;

        existingBooking.Status = ParseBookingStatus(status);
        existingBooking.UpdatedAt = DateTime.UtcNow;

        return await _bookingRepository.UpdateAsync(id, existingBooking);
    }

    public async Task<bool> DeleteBookingAsync(string id)
    {
        return await _bookingRepository.DeleteAsync(id);
    }

    private static BookingStatus ParseBookingStatus(string status)
    {
        return status.ToLower() switch
        {
            "reserved" => BookingStatus.Reserved,
            "checked-in" or "checkedin" => BookingStatus.CheckedIn,
            "checked-out" or "checkedout" => BookingStatus.CheckedOut,
            _ => BookingStatus.Reserved
        };
    }
}
