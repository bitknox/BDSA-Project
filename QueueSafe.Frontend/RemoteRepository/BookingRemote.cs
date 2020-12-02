namespace QueueSafe.Frontend
{
    public class BookingRemote
    {
        private readonly HttpClient _httpClient;
        public BookingRemote(HttpClient httpClient)
        {
             _httpClient = httpClient;
        }

        public void GetBooking(int Id)
        {}
    }
}

