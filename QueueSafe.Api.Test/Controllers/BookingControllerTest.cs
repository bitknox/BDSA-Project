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

        public BookingControllerTest()
        {
            var mockBookingRepository = new Mock<IBookingRepository>();
            mockBookingRepository.Setup(m => m.Read("sometoken")).ReturnsAsync(new BookingDetailsDTO());
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
    }
}