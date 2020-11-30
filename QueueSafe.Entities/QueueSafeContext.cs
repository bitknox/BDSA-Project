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
           
            modelBuilder.Entity<City>()
                    .HasKey(c => c.Postal);

            modelBuilder.Entity<Address>()
                    .HasOne<City>(a => a.City)
                    .WithMany(b => b.Addresses)
                    .HasForeignKey(a => a.CityPostal);
     
            modelBuilder.Entity<Address>()
                    .HasKey(s => new { s.StreetName, s.HouseNumber, s.CityPostal });


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