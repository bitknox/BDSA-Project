using System;
using QueueSafe.Entities;
using QueueSafe.Shared;
using Microsoft.EntityFrameworkCore;
using static System.Net.HttpStatusCode;
using System.Threading.Tasks;
using System.Net;
using System.Linq;

namespace QueueSafe.Models
{
    public class StoreRepository : IStoreRepository
    {
        private readonly IQueueSafeContext _context;

        public StoreRepository(IQueueSafeContext context)
        {
            _context = context;
        }

        public async Task<(int affectedRows, int id)> Create(StoreCreateDTO Store)
        {
            var entity = new Store
            {
                Capacity = Store.Capacity,
                Name = Store.Name,
                Address = new Address
                {
                    StreetName = Store.Address.StreetName,
                    City = await GetCity(Store.Address.City),
                    HouseNumber = Store.Address.HouseNumber
                },
                Image = Store.Image
            };

            _context.Store.Add(entity);
            var affectedRows = await _context.SaveChangesAsync();
            return (affectedRows, entity.Id);
        }

        public async Task<HttpStatusCode> Delete(int StoreId)
        {
            var entity = await _context.Store.FindAsync(StoreId);

            if (entity != null)
            {
                _context.Store.Remove(entity);
                await _context.SaveChangesAsync();
                return OK;
            }

            return NotFound;
        }

        public async Task<StoreDetailsDTO> Read(int StoreId)
        {
            var entity = from h in _context.Store
                         where h.Id == StoreId
                         select new StoreDetailsDTO
                         {
                             Name = h.Name,
                             Capacity = h.Capacity,
                             Address = h.Address.ToString()
                         };

            return await entity.FirstOrDefaultAsync();
        }

        public IQueryable<StoreListDTO> ReadAllStores()
        {
            var stores = from h in _context.Store
                         select new StoreListDTO
                         {
                             Id = h.Id,
                             Name = h.Name,
                             Capacity = h.Capacity
                         };

            return stores;
        }

        public IQueryable<StoreListDTO> ReadStoresCity(CityDTO City)
        {
            var stores = from h in _context.Store
                         where h.Address.City.Postal == City.Postal
                         select new StoreListDTO
                         {
                             Id = h.Id,
                             Name = h.Name,
                             Capacity = h.Capacity
                         };

            return stores;
        }

        public async Task<HttpStatusCode> Update(StoreUpdateDTO Store)
        {
            var entity = await _context.Store.FindAsync(Store.Id);

            if (entity == null) return NotFound;

            if (Store.Capacity > 0) entity.Capacity = Store.Capacity;
            else return BadRequest;

            await _context.SaveChangesAsync();

            return OK;
        }

        private async Task<City> GetCity(CityDTO City)
        {
            var entity = await _context.City.FirstOrDefaultAsync(c => c.Postal == City.Postal);
            Console.WriteLine(entity);
            if (entity == null) return new City { Name = City.Name, Postal = City.Postal };
            else return entity;
        }
    }
}
