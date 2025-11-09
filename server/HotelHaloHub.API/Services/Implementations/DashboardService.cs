using AutoMapper;
using HotelHaloHub.API.Models.DTOs;
using HotelHaloHub.API.Models.Entities;
using HotelHaloHub.API.Repositories.Interfaces;
using HotelHaloHub.API.Services.Interfaces;

namespace HotelHaloHub.API.Services.Implementations;

public class DashboardService : IDashboardService
{
    private readonly IRoomRepository _roomRepository;
    private readonly IBookingRepository _bookingRepository;
    private readonly IMapper _mapper;

    public DashboardService(IRoomRepository roomRepository, IBookingRepository bookingRepository, IMapper mapper)
    {
        _roomRepository = roomRepository;
        _bookingRepository = bookingRepository;
        _mapper = mapper;
    }

    public async Task<DashboardStatsDto> GetDashboardStatsAsync(string hotelId)
    {
        var rooms = (await _roomRepository.GetByHotelIdAsync(hotelId)).ToList();
        var totalRooms = rooms.Count;
        var availableRooms = rooms.Count(r => r.Status == RoomStatus.Available);
        var occupiedRooms = rooms.Count(r => r.Status == RoomStatus.Occupied);

        var occupancyRate = totalRooms > 0 ? (decimal)occupiedRooms / totalRooms * 100 : 0;

        var todayBookings = (await _bookingRepository.GetTodayBookingsAsync()).ToList();
        var bookingsToday = todayBookings.Count;

        var today = DateTime.UtcNow.Date;
        var checkInsToday = todayBookings.Count(b => b.CheckInDate.Date == today);
        var checkOutsToday = todayBookings.Count(b => b.CheckOutDate.Date == today);

        // Calculate monthly revenue (current month)
        var startOfMonth = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
        var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);
        var monthlyBookings = await _bookingRepository.GetBookingsByDateRangeAsync(startOfMonth, endOfMonth);
        var monthlyRevenue = monthlyBookings.Sum(b => b.TotalAmount);

        // Get recent bookings
        var recentBookings = await _bookingRepository.GetRecentBookingsAsync(10);
        var recentBookingDtos = new List<RecentBookingDto>();

        foreach (var booking in recentBookings)
        {
            var dto = _mapper.Map<RecentBookingDto>(booking);
            var room = await _roomRepository.GetByIdAsync(booking.RoomId);
            dto.Room = room?.Number ?? "Unknown";
            recentBookingDtos.Add(dto);
        }

        return new DashboardStatsDto
        {
            TotalRooms = totalRooms,
            AvailableRooms = availableRooms,
            OccupiedRooms = occupiedRooms,
            OccupancyRate = Math.Round(occupancyRate, 2),
            BookingsToday = bookingsToday,
            CheckInsToday = checkInsToday,
            CheckOutsToday = checkOutsToday,
            MonthlyRevenue = monthlyRevenue,
            RecentBookings = recentBookingDtos
        };
    }
}
