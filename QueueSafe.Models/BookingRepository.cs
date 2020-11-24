using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System;
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
            var entity = await _context.Booking.FindAsync(token);

            if (entity != null)
            {
                _context.Booking.Remove(entity);
                await _context.SaveChangesAsync();
                return OK;
            }

            return NotFound;
        }

        public async Task<BookingDetailsDTO> Read(string token)
        {
            var entity = from h in _context.Booking
                         where h.Token == token
                         select new BookingDetailsDTO
                         {
                             StoreId = h.StoreId,
                             StoreName = h.Store.Name,
                             TimeStamp = h.TimeStamp,
                             State = (QueueSafe.Shared.BookingState)h.State
                         };
            return await entity.FirstOrDefaultAsync();
        }

        public async Task<ICollection<BookingListDTO>> ReadAllBookings()
        {
            var bookings = from h in _context.Booking
                           select new BookingListDTO
                           {
                               StoreId = h.StoreId,
                               TimeStamp = h.TimeStamp,
                               Token = h.Token
                           };

            return await bookings.ToListAsync();
        }

        public async Task<ICollection<BookingListDTO>> ReadStoreBookings(int StoreId)
        {
            var bookings = from h in _context.Booking
                           where h.StoreId == StoreId
                           select new BookingListDTO
                           {
                               StoreId = h.StoreId,
                               TimeStamp = h.TimeStamp,
                               Token = h.Token
                           };

            return await bookings.ToListAsync();
        }

        public async Task<HttpStatusCode> Update(BookingUpdateDTO booking)
        {
            var entity = await _context.Booking.FindAsync(booking.Token);

            if (entity == null) return NotFound;

            entity.State = (QueueSafe.Entities.BookingState)booking.State;

            await _context.SaveChangesAsync();

            return OK;
        }

        private async Task<Store> GetStore(string name) => await _context.Store.FirstOrDefaultAsync(s => s.Name == name) ?? new Store { Name = name };
    }
}