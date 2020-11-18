using System.Text.Json.Serialization;

namespace QueueSafe.Shared
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

