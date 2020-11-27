using System;
using Xunit;
using QueueSafe.Models;
using Microsoft.EntityFrameworkCore;
using QueueSafe.Entities;
using Microsoft.Data.Sqlite;
using QueueSafe.Shared;
using System.Threading.Tasks;
using static System.Net.HttpStatusCode;

namespace QueueSafe.Models.Test
{
    public class BookingRepositoryTest : IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly QueueSafeContext _context;
        private readonly BookingRepository _repository;

        public BookingRepositoryTest()
        {
            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();
            var builder = new DbContextOptionsBuilder<QueueSafeContext>().UseSqlite(_connection);

            _context = new QueueSafeContext(builder.Options);
            _context.Database.EnsureCreated();
            _context.GenerateTestData();

            _repository = new BookingRepository(_context);
        }

        [Fact]
        public async Task Create_booking_successful_adds_zero_entities_to_db_from_nonexisting_store()
        {   
            // Act
            var result = await _repository.Create(236666663);

            // Assert
            Assert.Equal(0, result.affectedRows);
        }

        [Fact]
        public async Task Create_booking_successful_adds_one_entitie_to_db_from_existing_store()
        {
            // Act
            var result = await _repository.Create(2);

            // Assert
            Assert.Equal(1, result.affectedRows);
        }

        [Fact]
        public async Task Delete_existing_booking_successful()
        {
            // Act
            var actual = await _repository.Delete("goodtoken1");
            var expected = OK;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task Delete_non_existing_booking_unsuccessful()
        {
            // Arrange
            var badToken = "badToken";

            // Act
            var actual = await _repository.Delete(badToken);
            var expected = NotFound;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task Read_non_existing_booking_returns_null()
        {
            // Arange
            var badToken = "badToken";

            // Act
            var actual = await _repository.Read(badToken);

            // Assert
            Assert.Equal(null, actual);
        }

        [Fact]
        public async Task Read_existing_booking_successful()
        {
            // Arrange
            var goodToken = "goodtoken1";

            // Act
            var entity = await _repository.Read(goodToken);

            // Assert
            Assert.Equal(entity.StoreName, "GandalfsButtHashing");
        }

        [Fact]
        public async Task Read_returns_list_of_5()
        {
            // Act
            var entity = _repository.ReadAllBookings();
            var entityList = await entity.ToListAsync();

            // Assert
            Assert.Equal(5, entityList.Count);
        }

        [Fact]
        public async Task Read_for_Elminidik_returns_list_of_3()
        {
            // Arrange
            var entity = _repository.ReadStoreBookings(2);
            var entityList = await entity.ToListAsync();

            // Act
            var actual = entityList.Count;
            var expected = 3;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task Update_non_existing_booking_unsuccessful()
        {
            // Arrange
            var entity = new BookingUpdateDTO { Token = "badToken" };

            // Act
            var actual = await _repository.Update(entity);
            var expected = NotFound;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task Update_existing_booking_successful()
        {
            // Arrange
            var entity = new BookingUpdateDTO { Token = "goodtoken1", State = Shared.BookingState.Canceled };

            // Act
            var actualResponse = await _repository.Update(entity);
            var expectedResponse = OK;

            var updated = await _repository.Read(entity.Token);
            var actualState = updated.State;
            var expectedState = Shared.BookingState.Canceled;

            // Assert
            Assert.Equal(expectedResponse, actualResponse);
            Assert.Equal(expectedState, actualState);
        }


        public void Dispose()
        {
            _connection.Dispose();
            _context.Dispose();
        }
    }
}
