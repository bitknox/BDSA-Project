using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QueueSafe.Models;
using QueueSafe.Shared;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace QueueSafe.Api.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingRepository _repository;

        public BookingController(IBookingRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{token}")]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status404NotFound)]
        public async Task<ActionResult<BookingDetailsDTO>> Get(string token)
        {

            var booking = await _repository.Read(token);

            if (booking == null) return NotFound();

            return booking;
        }
    }
}
