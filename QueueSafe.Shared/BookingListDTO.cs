using System;

namespace QueueSafe.Shared
{
    public class BookingListDTO
    {
        public int StoreId { get; set; }
    
        public DateTime TimeStamp { get; set; }
        
        public string Token { get; set; } 

        public BookingState State { get; set; }
    }
}