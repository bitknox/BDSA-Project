using System.Text.Json.Serialization;

namespace QueueSafe.Entities
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum BookingState
    {
        Pending,
        Active,
        Canceled,
        Expired,
        Closed
    }
}

