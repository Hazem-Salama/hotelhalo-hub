using HotelHaloHub.API.Models.Entities;

namespace HotelHaloHub.API.Configuration;

public class InMemoryDataStore
{
    public List<Hotel> Hotels { get; set; } = new();
    public List<Room> Rooms { get; set; } = new();
    public List<Booking> Bookings { get; set; } = new();

    private static InMemoryDataStore? _instance;
    private static readonly object _lock = new();

    public static InMemoryDataStore Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new InMemoryDataStore();
                    }
                }
            }
            return _instance;
        }
    }

    private InMemoryDataStore()
    {
        // Private constructor to ensure singleton
    }
}
