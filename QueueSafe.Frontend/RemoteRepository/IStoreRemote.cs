using System.Threading.Tasks;
using QueueSafe.Shared;

namespace QueueSafe.Frontend
{
    public interface IStoreRemote
    {
        Task<StoreDetailsDTO> GetStore(string StoreId);
        Task<StoreListDTO[]> GetAllStores();
    }
}