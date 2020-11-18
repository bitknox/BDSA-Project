using System;

namespace QueueSafe.Shared
{
    public class BookingDetailsDTO
    {
        public int? StoreId { get; set; }

        public string StoreName { get; set; }
    
        public DateTime TimeStamp { get; set; }
    }
}