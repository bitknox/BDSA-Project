namespace QueueSafe.Shared
{
    public class AddressDTO
    {
        public int StoreId { get; set; }

        public string StreetName { get; set; }

        public CityDTO City { get; set; }

        public int HouseNumber { get; set; }
        
    }
}