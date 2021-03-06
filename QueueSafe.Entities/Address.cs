namespace QueueSafe.Entities
{
    public class Address
    {
        public Store Store { get; set; }
        
        public int StoreId { get; set; }

        public string StreetName { get; set; }

        public City City { get; set; }

        public int CityPostal { get; set; }

        public int HouseNumber { get; set; }

        public override string ToString() => $"{City.Postal} {City.Name}, {StreetName} {HouseNumber}";
    }
}