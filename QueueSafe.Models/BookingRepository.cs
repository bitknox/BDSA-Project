using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QueueSafe.Entities;
using QueueSafe.Shared;
using State = QueueSafe.Entities.BookingState;
using static System.Net.HttpStatusCode;

namespace QueueSafe.Models
{
    public class BookingRepository : IBookingRepository
    {
        private readonly IQueueSafeContext _context;

        public BookingRepository(IQueueSafeContext context)
        {
            _context = context;
        }

        public async Task<(int affectedRows, string token)> Create(int StoreId)
        {
            try
            {
                var entity = new Booking
                {
                    Store = await GetStore(StoreId),
                    Token = Guid.NewGuid().ToString(),
                    TimeStamp = DateTime.Now,
                    State = State.Pending
                };

                _context.Booking.Add(entity);
                var affectedRows = await _context.SaveChangesAsync();
                return (affectedRows, entity.Token);
            }
            catch (ArgumentException)
            {
                return (0, null);
            }
        }

        public async Task<HttpStatusCode> Delete(string Token)
        {
            var entity = await _context.Booking.FindAsync(Token);

            if (entity != null)
            {
                _context.Booking.Remove(entity);
                await _context.SaveChangesAsync();
                return OK;
            }

            return NotFound;
        }

        public async Task<BookingDetailsDTO> Read(string Token)
        {
            var entity = from h in _context.Booking
                         where h.Token == Token
                         select new BookingDetailsDTO
                         {
                             StoreId = h.StoreId,
                             StoreName = h.Store.Name,
                             TimeStamp = h.TimeStamp,
                             State = (QueueSafe.Shared.BookingState)h.State
                         };
            return await entity.FirstOrDefaultAsync();
        }

        public IQueryable<BookingListDTO> ReadAllBookings()
        {
            var bookings = from h in _context.Booking
                           select new BookingListDTO
                           {
                               StoreId = h.StoreId,
                               TimeStamp = h.TimeStamp,
                               Token = h.Token
                           };

            return bookings;
        }

        public IQueryable<BookingListDTO> ReadStoreBookings(int StoreId)
        {
            var bookings = from h in _context.Booking
                           where h.StoreId == StoreId
                           select new BookingListDTO
                           {
                               StoreId = h.StoreId,
                               TimeStamp = h.TimeStamp,
                               Token = h.Token
                           };

            return bookings;
        }

        public async Task<HttpStatusCode> Update(BookingUpdateDTO Booking)
        {
            var entity = await _context.Booking.FindAsync(Booking.Token);

            if (entity == null) return NotFound;

            entity.State = (QueueSafe.Entities.BookingState)Booking.State;

            await _context.SaveChangesAsync();

            return OK;
        }
        private async Task<Store> GetStore(int StoreId) => await _context.Store.FirstOrDefaultAsync(s => s.Id == StoreId) ?? throw new ArgumentException($"Unknown StoreId: {StoreId}");
    }
}