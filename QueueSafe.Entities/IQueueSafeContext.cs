using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace QueueSafe.Entities
{
    public interface IQueueSafeContext
    {
        DbSet<Booking> Booking { get; }
        DbSet<Store> Store { get; set; }
        DbSet<City> City { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}