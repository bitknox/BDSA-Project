using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace QueueSafe.Entities
{
    public interface IBookingContext
    {
        DbSet<Booking> Booking { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}