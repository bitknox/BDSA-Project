using System;
using Xunit;
using QueueSafe.Models;
using Microsoft.EntityFrameworkCore;
using QueueSafe.Entities;
using Microsoft.Data.Sqlite;
using QueueSafe.Shared;
using System.Threading.Tasks;

namespace QueueSafe.Models.Test
{
    public class BookingRepositoryTest : IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly BookingContext _context;
        private readonly BookingRepository _repository;

        public BookingRepositoryTest()
        {
            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();
            var builder = new DbContextOptionsBuilder<BookingContext>().UseSqlite(_connection);

            _context = new BookingContext(builder.Options);
            _context.Database.EnsureCreated();
            _context.GenerateTestData();

            _repository = new BookingRepository(_context);
        }

        [Fact]
        public async Task Create_booking_successful_adds_two_entities_to_db_from_nonexisting_store()
        {
            // Arrange
            var booking = new BookingCreateDTO
            {
                StoreName = "GandalfsButtHash",
                Token = "dikkerman"
            };

            // Act
            var result = await _repository.Create(booking);

            // Assert
            Assert.Equal(2, result);
        }

        [Fact]
        public async Task Create_booking_successful_adds_one_entitie_to_db_from_existing_store()
        {
            // Arrange
            var booking = new BookingCreateDTO
            {
                StoreName = "GandalfsButtHashing",
                Token = "shat"
            };

            // Act
            var result = await _repository.Create(booking);

            // Assert
            Assert.Equal(1, result);
        }

        public void Dispose()
        {
            _connection.Dispose();
            _context.Dispose();
        }
    }
}
