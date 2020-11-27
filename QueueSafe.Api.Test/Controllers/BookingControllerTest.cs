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
    public class BookingControllerTest
    {
        private readonly BookingController _controller;
        private readonly Mock<IBookingRepository> mockBookingRepository;

        public BookingControllerTest()
        {
            mockBookingRepository = new Mock<IBookingRepository>();
            mockBookingRepository.Setup(m => m.Read("sometoken")).ReturnsAsync(new BookingDetailsDTO());
            mockBookingRepository.Setup(m => m.ReadAllBookings()).Returns(new List<BookingListDTO>().AsQueryable());
            mockBookingRepository.Setup(m => m.Delete("wrongtoken")).ReturnsAsync(HttpStatusCode.NotFound);
            mockBookingRepository.Setup(m => m.Delete("righttoken")).ReturnsAsync(HttpStatusCode.OK);
            mockBookingRepository.Setup(m => m.Update(new BookingUpdateDTO{Token = "righttoken", State = BookingState.Expired})).ReturnsAsync(HttpStatusCode.OK);
            mockBookingRepository.Setup(m => m.Update(new BookingUpdateDTO{Token = "wrongtoken", State = BookingState.Expired})).ReturnsAsync(HttpStatusCode.NotFound);
            _controller = new BookingController(mockBookingRepository.Object);
        }

        [Fact]
        public async void Get_valid_booking_returns_booking_details()
        {
            // Act
            var actual = await _controller.Get("sometoken");

            // Assert
            Assert.IsType<BookingDetailsDTO>(actual.Value);
        }

        [Fact]
        public async void Get_invalid_booking_returns_not_found()
        {
            // Act
            var actual = await _controller.Get("nothin");

            // Assert
            Assert.IsType<NotFoundResult>(actual.Result);
        }

        [Fact]
        public void Get_all_bookings_returns_booking_list()
        {
            // Act
            var actual = _controller.Get();

            // Assert
            Assert.IsType<List<BookingListDTO>>(actual.Value);
        }
        
        [Fact]
        public async void Delete_existing_booking_returns_ok()
        {
            //Act
            var actual = await _controller.Delete("righttoken");
            
            // Assert
            var result = Assert.IsType<StatusCodeResult>(actual);

            Assert.Equal(200, result.StatusCode);   
        }

        [Fact]
        public async void Delete_non_existing_booking_returns_not_found()
        {
            // Act
            var actual = await _controller.Delete("wrongtoken");
             
            // Assert
            var result = Assert.IsType<StatusCodeResult>(actual);

            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async void Update_non_existing_booking_returns_ok()
        {
            // Arrange
            var updateBooking = new BookingUpdateDTO 
            {
                Token = "righttoken",
                State = BookingState.Expired
            };
            mockBookingRepository.Setup(m => m.Update(updateBooking)).ReturnsAsync(HttpStatusCode.OK);
            var controller = new BookingController(mockBookingRepository.Object);
            
            // Act
            var actual = await controller.Put(updateBooking.Token, updateBooking);

            // Assert
            var result = Assert.IsType<StatusCodeResult>(actual);
            
            Assert.Equal(200, result.StatusCode);
        }
        
        [Fact]
        public async void Update_non_existing_booking_returns_not_found()
        {
            // Arrange
            var updateBooking = new BookingUpdateDTO 
            {
                Token = "wrongtoken",
                State = BookingState.Expired
            };
            mockBookingRepository.Setup(m => m.Update(updateBooking)).ReturnsAsync(HttpStatusCode.NotFound);
            var controller = new BookingController(mockBookingRepository.Object);
            
            // Act
            var actual = await controller.Put(updateBooking.Token, updateBooking);

            // Assert
            var result = Assert.IsType<StatusCodeResult>(actual);
            
            Assert.Equal(404, result.StatusCode);
        }
    }
}    
