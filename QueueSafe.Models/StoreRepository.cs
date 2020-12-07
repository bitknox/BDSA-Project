using System.Collections.Generic;
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
            return await _context.Store
                    .Include(store => store.Address)
                    .Include(store => store.Address.City)
                    .Include(store => store.Bookings)
                    .Where(store => store.Id == StoreId)
                    .Select(store => new StoreDetailsDTO
                    {
                        Name = store.Name,
                        Capacity = store.Capacity,
                        Address = store.Address.ToString(),
                        Image = store.Image,
                        Bookings = store.Bookings.Select(booking => new BookingListDTO
                        {
                            Token = booking.Token,
                            StoreId = booking.StoreId,
                            TimeStamp = booking.TimeStamp,
                            State = (QueueSafe.Shared.BookingState) booking.State
                        }).ToList()
                    }).FirstOrDefaultAsync();
        }

        public IQueryable<StoreListDTO> ReadAllStores()
        {
            return _context.Store
                     .Include(store => store.Address)
                     .Include(store => store.Address.City)
                     .Select(store => new StoreListDTO
                     {
                         Id = store.Id,
                         Name = store.Name,
                         Capacity = store.Capacity,
                         Address = store.Address.ToString(),
                         Image = store.Image
                     });
        }

        public IQueryable<StoreListDTO> ReadStoresCity(CityDTO City)
        {
            return _context.Store
                    .Include(store => store.Address)
                    .Include(store => store.Address.City)
                    .Where(store => store.Address.City.Postal == City.Postal)
                    .Select(store => new StoreListDTO
                    {
                        Id = store.Id,
                        Name = store.Name,
                        Capacity = store.Capacity,
                        Address = store.Address.ToString(),
                        Image = store.Image
                    });
        }

        public async Task<HttpStatusCode> Update(StoreUpdateDTO Store)
        {
            var entity = await _context.Store.FindAsync(Store.Id);

            if (entity == null) return NotFound;

            if (Store.Capacity > 0) entity.Capacity = Store.Capacity;
            else return BadRequest;

            entity.Image = Store.Image;

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
