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
            var entity = await _repository.ReadAllBookings();

            // Assert
            Assert.Equal(5, entity.Count);
        }

        [Fact]
        public async Task Read_for_Elminidik_returns_list_of_3()
        {
            // Arrange
            var entity = await _repository.ReadStoreBookings(2);

            // Act
            var actual = entity.Count;
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