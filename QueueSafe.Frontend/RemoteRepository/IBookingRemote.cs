using QueueSafe.Shared;
using System.Threading.Tasks;

namespace QueueSafe.Frontend
{
    public interface IBookingRemote
    {
        Task<BookingDetailsDTO> GetBooking(string token);
        Task<BookingListDTO> CreateBooking(string StoreId);
    }
}