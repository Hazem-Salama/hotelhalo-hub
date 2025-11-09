namespace HotelHaloHub.API.Models.DTOs;

public class DashboardStatsDto
{
    public int TotalRooms { get; set; }
    public int AvailableRooms { get; set; }
    public int OccupiedRooms { get; set; }
    public decimal OccupancyRate { get; set; }
    public int BookingsToday { get; set; }
    public int CheckInsToday { get; set; }
    public int CheckOutsToday { get; set; }
    public decimal MonthlyRevenue { get; set; }
    public List<RecentBookingDto> RecentBookings { get; set; } = new();
}

public class RecentBookingDto
{
    public string Id { get; set; } = string.Empty;
    public string Guest { get; set; } = string.Empty;
    public string Room { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string CheckIn { get; set; } = string.Empty;
    public string CheckOut { get; set; } = string.Empty;
}
