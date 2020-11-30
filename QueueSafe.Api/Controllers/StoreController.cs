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
    public class StoreController : ControllerBase
    {
        private readonly IStoreRepository _repository;

        public StoreController(IStoreRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        [ProducesResponseType(Status201Created)]
        [ProducesResponseType(Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] StoreCreateDTO store)
        {            
            var result = await _repository.Create(store);

            return CreatedAtAction(nameof(Get), new { result.id }, default);
        }

        [HttpGet("{Id}")]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status404NotFound)]
        public async Task<ActionResult<StoreDetailsDTO>> Get(int StoreId)
        {
            var store = await _repository.Read(StoreId);

            if (store == null) return NotFound();

            return store;
        }

        [HttpGet("{Postal}")]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status404NotFound)]
        public ActionResult<IEnumerable<StoreListDTO>> Get(CityDTO City)
        {
            return _repository.ReadStoresCity(City).ToList();
        }
    
        [HttpGet("all")]
        [ProducesResponseType(Status200OK)]
        public ActionResult<IEnumerable<StoreListDTO>> Get()
        {
            return _repository.ReadAllStores().ToList();
        }

        [HttpPut("{Id}")]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status404NotFound)]
        public async Task<ActionResult> Put(int Id, [FromBody] StoreUpdateDTO Store)        
        {
            // Ensures you dont update wrong object
            if (Id != Store.Id)
            {
                ModelState.AddModelError("Id", "Id in URL must match Id in body");

                return BadRequest(ModelState);
            }
            
            var response = await _repository.Update(Store);

            return new StatusCodeResult((int)response);
        }

        [HttpDelete("{Id}")]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status404NotFound)]
        public async Task<ActionResult> Delete(int Id)
        {
            var response = await _repository.Delete(Id);

            return new StatusCodeResult((int) response);
        }
    }
}
