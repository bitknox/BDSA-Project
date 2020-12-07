using Microsoft.AspNetCore.Mvc;
using QueueSafe.Shared;
using System.Net;
using System.Threading.Tasks;

namespace QueueSafe.Frontend
{
    public interface IBookingRemote
    {
        Task<BookingDetailsDTO> GetBooking(string token);
        Task<BookingListDTO> CreateBooking(string StoreId);
        Task<bool> UpdateBooking(string token, BookingUpdateDTO booking);
        Task<bool> DeleteBooking(string token);
    }
}