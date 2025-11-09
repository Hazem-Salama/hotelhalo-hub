using AutoMapper;
using HotelHaloHub.API.Models.DTOs;
using HotelHaloHub.API.Models.Entities;
using HotelHaloHub.API.Repositories.Interfaces;
using HotelHaloHub.API.Services.Interfaces;

namespace HotelHaloHub.API.Services.Implementations;

public class HotelService : IHotelService
{
    private readonly IHotelRepository _hotelRepository;
    private readonly IMapper _mapper;

    public HotelService(IHotelRepository hotelRepository, IMapper mapper)
    {
        _hotelRepository = hotelRepository;
        _mapper = mapper;
    }

    public async Task<HotelDto?> GetHotelSettingsAsync()
    {
        var hotel = await _hotelRepository.GetFirstAsync();
        return hotel == null ? null : _mapper.Map<HotelDto>(hotel);
    }

    public async Task<HotelDto> UpdateHotelSettingsAsync(string id, UpdateHotelDto updateHotelDto)
    {
        var existingHotel = await _hotelRepository.GetByIdAsync(id);
        if (existingHotel == null)
            throw new Exception("Hotel not found");

        // Update only provided fields
        if (!string.IsNullOrEmpty(updateHotelDto.Name))
            existingHotel.Name = updateHotelDto.Name;

        if (!string.IsNullOrEmpty(updateHotelDto.Address))
            existingHotel.Address = updateHotelDto.Address;

        if (!string.IsNullOrEmpty(updateHotelDto.Phone))
            existingHotel.Phone = updateHotelDto.Phone;

        if (!string.IsNullOrEmpty(updateHotelDto.Email))
            existingHotel.Email = updateHotelDto.Email;

        existingHotel.UpdatedAt = DateTime.UtcNow;

        await _hotelRepository.UpdateAsync(id, existingHotel);
        return _mapper.Map<HotelDto>(existingHotel);
    }

    public async Task<string> GetOrCreateDefaultHotelIdAsync()
    {
        var hotel = await _hotelRepository.GetFirstAsync();

        if (hotel != null)
            return hotel.Id;

        // Create default hotel
        var defaultHotel = new Hotel
        {
            Name = "Grand Hotel",
            Address = "123 Main Street, City",
            Phone = "+1 234 567 8900",
            Email = "info@grandhotel.com",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var createdHotel = await _hotelRepository.CreateAsync(defaultHotel);
        return createdHotel.Id;
    }
}
