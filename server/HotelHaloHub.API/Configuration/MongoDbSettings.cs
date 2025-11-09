namespace HotelHaloHub.API.Configuration;

public class MongoDbSettings
{
    public string ConnectionString { get; set; } = string.Empty;
    public string DatabaseName { get; set; } = string.Empty;
    public string HotelsCollectionName { get; set; } = "hotels";
    public string RoomsCollectionName { get; set; } = "rooms";
    public string BookingsCollectionName { get; set; } = "bookings";
}
