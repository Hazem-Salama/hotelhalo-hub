namespace HotelHaloHub.API.Models.Entities;

public class Room
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string HotelId { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty; // Standard, Deluxe, Suite
    public RoomStatus Status { get; set; } = RoomStatus.Available;
    public decimal Price { get; set; }
    public int Capacity { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

public enum RoomStatus
{
    Available,
    Occupied,
    Maintenance
}
