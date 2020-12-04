using System.Net.Http;
using System.Net.Http.Json;
using QueueSafe.Shared;
using System.Threading.Tasks;
using System;
using System.Text.Json;

namespace QueueSafe.Frontend
{
    public class BookingRemote : IBookingRemote
    {
        private readonly HttpClient _httpClient;

        public BookingRemote(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<BookingListDTO> CreateBooking(string StoreId)
        {
            var result = await _httpClient.PostAsync($"booking/{StoreId}", new StringContent(""));
            var response = JsonSerializer.Deserialize<BookingListDTO>(await result.Content.ReadAsStringAsync());
            return response;
        }

        public async Task<BookingDetailsDTO> GetBooking(string token)
        {
            return await _httpClient.GetFromJsonAsync<BookingDetailsDTO>($"booking/{token}");
        }
    }
}

