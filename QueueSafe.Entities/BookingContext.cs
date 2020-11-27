using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore.SqlServer;


namespace QueueSafe.Entities
{
    public class BookingContext : DbContext, IQueueSafeContext
    {
        public DbSet<Booking> Booking { get; set; }
        public DbSet<Store> Store { get; set; }


        public BookingContext(DbContextOptions<BookingContext> options)
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

            /*
            var Store = new Store
            {
                Id = 1,
                Capacity = 50,
                Name = "ElGigadik"
            };

            modelBuilder.Entity<Store>().HasData(Store);

            var Bookings = new[]
            {
                new Booking { StoreId = 1, TimeStamp = DateTime.Now, Token = "hbkHBAKBKHSDS/" },
                new Booking { StoreId = 1, TimeStamp = DateTime.Now, Token = "hbkHBasdAKBKHSDS/" }
            };

            modelBuilder.Entity<Booking>().HasData(Bookings);
            */
        }
    }
}