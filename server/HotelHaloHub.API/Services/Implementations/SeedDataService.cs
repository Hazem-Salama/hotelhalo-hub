using HotelHaloHub.API.Models.Entities;
using HotelHaloHub.API.Repositories.Interfaces;

namespace HotelHaloHub.API.Services.Implementations;

public class SeedDataService
{
    private readonly IHotelRepository _hotelRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IBookingRepository _bookingRepository;
    private readonly ILogger<SeedDataService> _logger;

    public SeedDataService(
        IHotelRepository hotelRepository,
        IRoomRepository roomRepository,
        IBookingRepository bookingRepository,
        ILogger<SeedDataService> logger)
    {
        _hotelRepository = hotelRepository;
        _roomRepository = roomRepository;
        _bookingRepository = bookingRepository;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        try
        {
            // Check if data already exists
            var existingHotel = await _hotelRepository.GetFirstAsync();
            if (existingHotel != null)
            {
                _logger.LogInformation("Database already seeded");
                return;
            }

            _logger.LogInformation("Starting database seeding...");

            // Create default hotel
            var hotel = new Hotel
            {
                Name = "Grand Hotel",
                Address = "123 Main Street, City",
                Phone = "+1 234 567 8900",
                Email = "info@grandhotel.com",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            hotel = await _hotelRepository.CreateAsync(hotel);
            _logger.LogInformation("Created hotel: {HotelName}", hotel.Name);

            // Create rooms
            var rooms = new List<Room>
            {
                new() { HotelId = hotel.Id, Number = "101", Type = "Standard", Status = RoomStatus.Available, Price = 120, Capacity = 2, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new() { HotelId = hotel.Id, Number = "102", Type = "Standard", Status = RoomStatus.Occupied, Price = 120, Capacity = 2, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new() { HotelId = hotel.Id, Number = "201", Type = "Deluxe", Status = RoomStatus.Available, Price = 180, Capacity = 3, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new() { HotelId = hotel.Id, Number = "202", Type = "Deluxe", Status = RoomStatus.Available, Price = 180, Capacity = 3, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new() { HotelId = hotel.Id, Number = "301", Type = "Suite", Status = RoomStatus.Occupied, Price = 280, Capacity = 4, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new() { HotelId = hotel.Id, Number = "302", Type = "Suite", Status = RoomStatus.Maintenance, Price = 280, Capacity = 4, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
            };

            foreach (var room in rooms)
            {
                await _roomRepository.CreateAsync(room);
            }
            _logger.LogInformation("Created {RoomCount} rooms", rooms.Count);

            // Get created rooms for bookings
            var room101 = await _roomRepository.GetByNumberAsync("101");
            var room102 = await _roomRepository.GetByNumberAsync("102");
            var room201 = await _roomRepository.GetByNumberAsync("201");
            var room301 = await _roomRepository.GetByNumberAsync("301");

            // Create bookings
            var bookings = new List<Booking>
            {
                new()
                {
                    HotelId = hotel.Id,
                    RoomId = room102!.Id,
                    GuestName = "John Doe",
                    GuestEmail = "john@example.com",
                    CheckInDate = DateTime.UtcNow.Date.AddDays(-1),
                    CheckOutDate = DateTime.UtcNow.Date.AddDays(1),
                    Status = BookingStatus.CheckedIn,
                    TotalAmount = 240,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new()
                {
                    HotelId = hotel.Id,
                    RoomId = room201!.Id,
                    GuestName = "Jane Smith",
                    GuestEmail = "jane@example.com",
                    CheckInDate = DateTime.UtcNow.Date,
                    CheckOutDate = DateTime.UtcNow.Date.AddDays(3),
                    Status = BookingStatus.Reserved,
                    TotalAmount = 540,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new()
                {
                    HotelId = hotel.Id,
                    RoomId = room301!.Id,
                    GuestName = "Mike Johnson",
                    GuestEmail = "mike@example.com",
                    CheckInDate = DateTime.UtcNow.Date.AddDays(-2),
                    CheckOutDate = DateTime.UtcNow.Date,
                    Status = BookingStatus.CheckedIn,
                    TotalAmount = 560,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            };

            foreach (var booking in bookings)
            {
                await _bookingRepository.CreateAsync(booking);
            }
            _logger.LogInformation("Created {BookingCount} bookings", bookings.Count);

            _logger.LogInformation("Database seeding completed successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while seeding database");
            throw;
        }
    }
}
