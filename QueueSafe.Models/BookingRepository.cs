using System.Linq;
using System.Net;
using System.Threading.Tasks;
using QueueSafe.Entities;
using QueueSafe.Shared;

namespace QueueSafe.Models 
{   //TODO: Implement methods :)
    public class BookingRepository : IBookingRepository
    {
        private readonly IBookingContext _context;

        public BookingRepository(IBookingContext context)
        {
            _context = context;
        }

        public Task<int> Create(BookingCreateDTO booking)
        {
            throw new System.NotImplementedException();
            // Default BookingState :D
        }

        public Task<HttpStatusCode> Delete(string token)
        {
            throw new System.NotImplementedException();
        }

        public Task<BookingDetailsDTO> Read(string token)
        {
            throw new System.NotImplementedException();
        }

        public IQueryable<BookingListDTO> ReadAllBookings()
        {
            throw new System.NotImplementedException();
        }

        public IQueryable<BookingListDTO> ReadStoreBookings(int StoreId)
        {
            throw new System.NotImplementedException();
        }

        public Task<HttpStatusCode> Update(BookingUpdateDTO booking)
        {
            throw new System.NotImplementedException();
        }
    }
}