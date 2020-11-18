using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Linq;
using QueueSafe.Shared;

namespace QueueSafe.Models
{
    public interface IBookingRepository
    {
        Task<int> Create(BookingCreateDTO booking);
        Task<BookingDetailsDTO> Read(string token);
        IQueryable<BookingListDTO> ReadAllBookings();
        IQueryable<BookingListDTO> ReadStoreBookings(int StoreId);
        Task<HttpStatusCode> Update(BookingUpdateDTO booking);
        Task<HttpStatusCode> Delete(string token);
    }
}
