using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QueueSafe.Models;
using QueueSafe.Shared;
using Microsoft.EntityFrameworkCore;
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

        [HttpPost("{StoreId}")]
        [ProducesResponseType(Status201Created)]
        [ProducesResponseType(Status400BadRequest)]
        public async Task<ActionResult<BookingListDTO>> Post(int StoreId)
        {
            var result = await _repository.Create(StoreId);
            if(result == null) return BadRequest();
            return result;
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
    
        [HttpGet("all")]
        [ProducesResponseType(Status200OK)]
        public ActionResult<IEnumerable<BookingListDTO>> Get()
        {
            return _repository.ReadAllBookings().ToList();
        }

        [HttpPut("{token}")]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status404NotFound)]
        public async Task<ActionResult> Put(string token, [FromBody] BookingUpdateDTO booking)        
        {
            // Ensures you dont update wrong object
            if (token != booking.Token)
            {
                ModelState.AddModelError("token", "token in URL must match token in body");

                return BadRequest(ModelState);
            }
            var response = await _repository.Update(booking);

            return new StatusCodeResult((int)response);
        }

        [HttpDelete("{token}")]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status404NotFound)]
        public async Task<ActionResult> Delete(string token)
        {
            var response = await _repository.Delete(token);

            return new StatusCodeResult((int) response);
        }
    }
}
