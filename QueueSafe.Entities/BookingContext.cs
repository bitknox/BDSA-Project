using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace QueueSafe.Entities
{
    public class BookingContext : DbContext, IBookingContext
    {
        public DbSet<Booking> Booking { get; set; }


        public BookingContext(DbContextOptions<BookingContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Booking>()
                        .HasIndex(s => s.Name)
                        .IsUnique();
        }
    }
}