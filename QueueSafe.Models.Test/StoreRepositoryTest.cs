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
    public class StoreRepositoryTest : IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly QueueSafeContext _context;
        private readonly StoreRepository _repository;

        public StoreRepositoryTest()
        {
            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();
            var builder = new DbContextOptionsBuilder<QueueSafeContext>().UseSqlite(_connection);

            _context = new QueueSafeContext(builder.Options);
            _context.Database.EnsureCreated();
            _context.GenerateTestData();

            _repository = new StoreRepository(_context);
        }
        
        [Fact]
        public async Task Create_store_successful_adds_three_entity_to_db()
        {
            // Arrange
            var store = new StoreCreateDTO {
                Name = "elminidik",
                Capacity = 50,
                Address = new AddressDTO { StreetName = "vejvej", City = new CityDTO { Postal = 2500 }, HouseNumber = 20 }
            };

            // Act
            var result = await _repository.Create(store);

            // Assert
            Assert.Equal(3, result.affectedRows);
        }

        [Fact]
        public async Task Delete_existing_store_successful()
        {
            // Arrange
            var expected = OK;
            
            // Act
            var actual = await _repository.Delete(1);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task Delete_non_existing_store_unsuccessful()
        {
            // Arrange
            var expected = NotFound;

            // Act
            var actual = await _repository.Delete(3);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task Read_non_existing_store_returns_null()
        {
            // Act
            var result = await _repository.Read(5);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Read_existing_store_successful()
        {
            // Arrange
            var expected = "ElMinidik";

            // Act
            var result = await _repository.Read(2);

            // Assert
            Assert.Equal(expected, result.Name);
        }

        [Fact]
        public async Task Read_returns_list_of_2()
        {
            // Arrange
            var expected = 2;

            // Act
            var results = await _repository.ReadAllStores().ToListAsync();

            // Assert
            Assert.Equal(expected, results.Count);
        }

        [Fact]
        public async Task Read_for_Postal1000_returns_list_of_1()
        {
            // Arrange
            var expected = 1;
            var city = new CityDTO { Name = "Skattejagten", Postal = 1000};

            // Act
            var results = await _repository.ReadStoresCity(city).ToListAsync();

            // Assert
            Assert.Equal(expected, results.Count);
        }

        [Fact]
        public async Task Update_non_existing_store_unsuccessful()
        {
            // Arrange
            var expected = NotFound;
            var store = new StoreUpdateDTO {
                Id = 5,
                Capacity = 69
            };

            // Act
            var result = await _repository.Update(store);

            // Assert
            Assert.Equal(expected, result);
        }


        [Fact]
        public async Task Update_existing_store_successful()
        {
            // Arrange
            var expected = OK;
            var store = new StoreUpdateDTO {
                Id = 1,
                Capacity = 200
            };

            // Act
            var result = await _repository.Update(store);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task Update_existing_store_with_less_than_0_capacity()
        {
            // Arrange
            var expected = BadRequest;
            var store = new StoreUpdateDTO {
                Id = 1,
                Capacity = -100
            };

            // Act
            var result = await _repository.Update(store);

            // Assert
            Assert.Equal(expected, result);
        }

        public void Dispose()
        {
            _connection.Dispose();
            _context.Dispose();
        }
    }
}
