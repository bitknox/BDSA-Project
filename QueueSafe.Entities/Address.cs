namespace QueueSafe.Entities
{
    public class Address
    {
        public Store Store { get; set; }
        
        public int StoreId { get; set; }

        public string StreetName { get; set; }

        public int Postal { get; set; }

        public int HouseNumber { get; set; }
        
        public override string ToString() => $"{Postal}, {StreetName} {HouseNumber}";
    }
}