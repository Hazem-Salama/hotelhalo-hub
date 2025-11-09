using HotelHaloHub.API.Models.DTOs;

namespace HotelHaloHub.API.Services.Interfaces;

public interface IDashboardService
{
    Task<DashboardStatsDto> GetDashboardStatsAsync(string hotelId);
}
