using System.Net.Http;
using QueueSafe.Shared;
using System.Threading.Tasks;
using System;

namespace QueueSafe.Frontend
{
    public class BookingRemote : IBookingRemote
    {
        private readonly HttpClient _httpClient;

        public BookingRemote(HttpClient httpClient)
        {
             _httpClient = httpClient;
        }

        public async Task<BookingDetailsDTO> GetBooking(string token)
        {
            throw new NotImplementedException();
        }
    }
}

