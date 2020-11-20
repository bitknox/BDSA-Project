using System;
using System.Security.Cryptography.X509Certificates;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QueueSafe.Entities;
using QueueSafe.Shared;
using State = QueueSafe.Entities.BookingState;

namespace QueueSafe.Models 
{ 
    public class BookingRepository : IBookingRepository
    {
        private readonly IBookingContext _context;

        public BookingRepository(IBookingContext context)
        {
            _context = context;
        }

        public async Task<int> Create(BookingCreateDTO booking)
        {
            var entity = new Booking
            {
                Store = await GetStore(booking.StoreName),
                Token = booking.Token,
                TimeStamp = booking.TimeStamp,
                State = State.Pending
            };

            _context.Booking.Add(entity);
            return await _context.SaveChangesAsync();
        }

        public async Task<HttpStatusCode> Delete(string token)
        {
            throw new System.NotImplementedException();
        }

        public async Task<BookingDetailsDTO> Read(string token)
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

        public async Task<HttpStatusCode> Update(BookingUpdateDTO booking)
        {
            throw new System.NotImplementedException();
        }

        private async Task<Store> GetStore(string name) => await _context.Store.FirstOrDefaultAsync(s => s.Name == name) ?? new Store { Name = name };
    }
}