using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore.SqlServer;


namespace QueueSafe.Entities
{
    public class QueueSafeContext : DbContext, IQueueSafeContext
    {
        public DbSet<Booking> Booking { get; set; }
        public DbSet<Store> Store { get; set; }


        public QueueSafeContext(DbContextOptions<QueueSafeContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Booking>()
                    .HasKey(s => s.Token);

            modelBuilder.Entity<Store>()
                    .HasKey(s => s.Id);

            modelBuilder.Entity<Store>()
                    .HasMany(c => c.Bookings)
                    .WithOne(c => c.Store);

            modelBuilder.Entity<Store>()
                    .HasOne(a => a.Address)
                    .WithOne(b => b.Store)
                    .HasForeignKey<Address>(b => b.StoreId);
            
            modelBuilder.Entity<Address>()
                    .HasKey(s => new { s.Postal, s.StreetName, s.HouseNumber });


        }
    }
}