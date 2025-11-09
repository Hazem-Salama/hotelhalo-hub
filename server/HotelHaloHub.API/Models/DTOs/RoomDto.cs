namespace HotelHaloHub.API.Models.DTOs;

public class RoomDto
{
    public string Id { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Capacity { get; set; }
}

public class CreateRoomDto
{
    public string Number { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Status { get; set; } = "available";
    public decimal Price { get; set; }
    public int Capacity { get; set; }
}

public class UpdateRoomDto
{
    public string? Number { get; set; }
    public string? Type { get; set; }
    public string? Status { get; set; }
    public decimal? Price { get; set; }
    public int? Capacity { get; set; }
}
