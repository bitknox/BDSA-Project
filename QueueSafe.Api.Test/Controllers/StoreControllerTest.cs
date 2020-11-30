using System.Net;
using System.Linq;
using System;
using Xunit;
using Moq;
using System.Collections.Generic;
using QueueSafe.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using QueueSafe.Models;
using QueueSafe.Shared;

namespace QueueSafe.Api.Test
{
    public class StoreControllerTest
    {
        private readonly StoreController _controller;
        private readonly Mock<IStoreRepository> mockStoreRepository;

        public StoreControllerTest()
        {
            mockStoreRepository = new Mock<IStoreRepository>();
            mockStoreRepository.Setup(m => m.Read(1)).ReturnsAsync(new StoreDetailsDTO());
            mockStoreRepository.Setup(m => m.ReadAllStores()).Returns(new List<StoreListDTO>().AsQueryable());
            mockStoreRepository.Setup(m => m.Delete(2)).ReturnsAsync(HttpStatusCode.NotFound);
            mockStoreRepository.Setup(m => m.Delete(1)).ReturnsAsync(HttpStatusCode.OK);
            _controller = new StoreController(mockStoreRepository.Object);
        }

        [Fact]
        public async void Get_valid_Store_returns_Store_details()
        {
            // Act
            var actual = await _controller.Get(1);

            // Assert
            Assert.IsType<StoreDetailsDTO>(actual.Value);
        }

        [Fact]
        public async void Get_invalid_Store_returns_not_found()
        {
            // Act
            var actual = await _controller.Get(2);

            // Assert
            Assert.IsType<NotFoundResult>(actual.Result);
        }

        [Fact]
        public void Get_all_Stores_returns_Store_list()
        {
            // Act
            var actual = _controller.Get();

            // Assert
            Assert.IsType<List<StoreListDTO>>(actual.Value);
        }

        [Fact]
        public void Get_Store_from_city_returns_Store_list()
        {
            // Arrange
            var city = new CityDTO 
            {
                Name = "TestCity",
                Postal = 6969
            };
            
            mockStoreRepository.Setup(m => m.ReadStoresCity(city)).Returns(new List<StoreListDTO>().AsQueryable());
            var controller = new StoreController(mockStoreRepository.Object);

            // Act
            var actual = controller.Get(city);

            // Assert
            Assert.IsType<List<StoreListDTO>>(actual.Value);
        }
        
        [Fact]
        public async void Delete_existing_Store_returns_ok()
        {
            //Act
            var actual = await _controller.Delete(1);
            
            // Assert
            var result = Assert.IsType<StatusCodeResult>(actual);

            Assert.Equal(200, result.StatusCode);   
        }

        [Fact]
        public async void Delete_non_existing_Store_returns_not_found()
        {
            // Act
            var actual = await _controller.Delete(2);
             
            // Assert
            var result = Assert.IsType<StatusCodeResult>(actual);

            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async void Update_existing_Store_returns_ok()
        {
            // Arrange
            var updateStore = new StoreUpdateDTO 
            {
                Id = 1,
                Capacity = 50
            };
            mockStoreRepository.Setup(m => m.Update(updateStore)).ReturnsAsync(HttpStatusCode.OK);
            var controller = new StoreController(mockStoreRepository.Object);
            
            // Act
            var actual = await controller.Put(updateStore.Id, updateStore);

            // Assert
            var result = Assert.IsType<StatusCodeResult>(actual);
            
            Assert.Equal(200, result.StatusCode);
        }
        
        [Fact]
        public async void Update_non_existing_Store_returns_not_found()
        {
            // Arrange
            var updateStore = new StoreUpdateDTO 
            {
                Id = 3,
                Capacity = 50
            };
            mockStoreRepository.Setup(m => m.Update(updateStore)).ReturnsAsync(HttpStatusCode.NotFound);
            var controller = new StoreController(mockStoreRepository.Object);
            
            // Act
            var actual = await controller.Put(updateStore.Id, updateStore);

            // Assert
            var result = Assert.IsType<StatusCodeResult>(actual);
            
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async void Create_Store_returns_created()
        {
            // Arrange
            var store = new StoreCreateDTO
            {
                Name = "StrongHashPipes",
                Capacity = 420,
                Address = new AddressDTO 
                {
                    StreetName = "CrackStreet",
                    City = new CityDTO { Name = "NewBong", Postal = 4202},
                    HouseNumber = 69
                }
            };

            mockStoreRepository.Setup(m => m.Create(store)).ReturnsAsync((1, 3));
            var controller = new StoreController(mockStoreRepository.Object);
            
            // Act
            var actual = await controller.Post(store);

             // Assert
            var result = Assert.IsType<CreatedAtActionResult>(actual);
            
            Assert.Equal(201, result.StatusCode);
        }
    }
}    
