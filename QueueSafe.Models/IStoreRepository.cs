using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Linq;
using QueueSafe.Shared;

namespace QueueSafe.Models
{
    public interface IStoreRepository
    {
        Task<int> Create(StoreCreateDTO Store);
        Task<StoreDetailsDTO> Read(int StoreId);
        IQueryable<StoreListDTO> ReadAllStores();
        IQueryable<StoreListDTO> ReadStoresCity(int Postal);
        Task<HttpStatusCode> Update(StoreUpdateDTO Store);
        Task<HttpStatusCode> Delete(int StoreId);
    }
}
