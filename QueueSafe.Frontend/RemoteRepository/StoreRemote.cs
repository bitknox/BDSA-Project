using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using QueueSafe.Shared;

namespace QueueSafe.Frontend
{
    public class StoreRemote : IStoreRemote
    {
        private readonly HttpClient _httpClient;

        public StoreRemote(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<StoreDetailsDTO> GetStore(string StoreId)
        {
            return await _httpClient.GetFromJsonAsync<StoreDetailsDTO>($"store/{StoreId}");
        }

        public async Task<StoreListDTO[]> GetAllStores()
        {
            return await _httpClient.GetFromJsonAsync<StoreListDTO[]>("store/all");
        }
    }
}