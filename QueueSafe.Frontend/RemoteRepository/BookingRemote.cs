using System.Net.Http;
using System.Net.Http.Json;
using QueueSafe.Shared;
using System.Threading.Tasks;
using System;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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

        public async Task<bool> DeleteBooking(string token)
        {
            var result = await _httpClient.DeleteAsync($"booking/{token}");
            var StatusCode = result.StatusCode;
            return StatusCode != HttpStatusCode.OK ? false : true;
        }

        public async Task<BookingDetailsDTO> GetBooking(string token)
        {
            return await _httpClient.GetFromJsonAsync<BookingDetailsDTO>($"booking/{token}");
        }

        public async Task<bool> UpdateBooking(string token, BookingUpdateDTO booking)
        {
            var result = await _httpClient.PutAsJsonAsync($"booking/{token}", booking);
            var StatusCode = result.StatusCode;
            return StatusCode != HttpStatusCode.OK ? false : true;
        }
    }
}

