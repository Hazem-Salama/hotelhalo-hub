using AutoMapper;
using HotelHaloHub.API.Models.DTOs;
using HotelHaloHub.API.Models.Entities;

namespace HotelHaloHub.API.Configuration;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        // Room mappings
        CreateMap<Room, RoomDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString().ToLower()));

        CreateMap<CreateRoomDto, Room>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => ParseRoomStatus(src.Status)))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));

        // Booking mappings
        CreateMap<Booking, BookingDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => MapBookingStatus(src.Status)))
            .ForMember(dest => dest.CheckIn, opt => opt.MapFrom(src => src.CheckInDate.ToString("yyyy-MM-dd")))
            .ForMember(dest => dest.CheckOut, opt => opt.MapFrom(src => src.CheckOutDate.ToString("yyyy-MM-dd")));

        CreateMap<CreateBookingDto, Booking>()
            .ForMember(dest => dest.CheckInDate, opt => opt.MapFrom(src => DateTime.Parse(src.CheckIn)))
            .ForMember(dest => dest.CheckOutDate, opt => opt.MapFrom(src => DateTime.Parse(src.CheckOut)))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => BookingStatus.Reserved))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));

        // Hotel mappings
        CreateMap<Hotel, HotelDto>();
        CreateMap<CreateHotelDto, Hotel>()
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));

        // Recent booking mappings
        CreateMap<Booking, RecentBookingDto>()
            .ForMember(dest => dest.Guest, opt => opt.MapFrom(src => src.GuestName))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => MapBookingStatus(src.Status)))
            .ForMember(dest => dest.CheckIn, opt => opt.MapFrom(src => src.CheckInDate.ToString("yyyy-MM-dd")))
            .ForMember(dest => dest.CheckOut, opt => opt.MapFrom(src => src.CheckOutDate.ToString("yyyy-MM-dd")));
    }

    private static RoomStatus ParseRoomStatus(string status)
    {
        return status.ToLower() switch
        {
            "available" => RoomStatus.Available,
            "occupied" => RoomStatus.Occupied,
            "maintenance" => RoomStatus.Maintenance,
            _ => RoomStatus.Available
        };
    }

    private static string MapBookingStatus(BookingStatus status)
    {
        return status switch
        {
            BookingStatus.Reserved => "reserved",
            BookingStatus.CheckedIn => "checked-in",
            BookingStatus.CheckedOut => "checked-out",
            _ => "reserved"
        };
    }
}
