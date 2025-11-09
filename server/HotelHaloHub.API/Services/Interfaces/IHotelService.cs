using HotelHaloHub.API.Models.DTOs;

namespace HotelHaloHub.API.Services.Interfaces;

public interface IHotelService
{
    Task<HotelDto?> GetHotelSettingsAsync();
    Task<HotelDto> UpdateHotelSettingsAsync(string id, UpdateHotelDto updateHotelDto);
    Task<string> GetOrCreateDefaultHotelIdAsync();
}
