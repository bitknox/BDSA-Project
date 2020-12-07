using QueueSafe.Shared;
using System.Linq;

namespace QueueSafe.Frontend.Extensions
{
    public static class DTOExtensions
    {
        public static bool IsFull(this StoreDetailsDTO store)
        {
            var activeBookingAmount = store.Bookings.Select(booking => booking).Where(booking => booking.State == BookingState.Active).Count();

            if(activeBookingAmount >= store.Capacity) return true;

            return false;
        } 
    }
}