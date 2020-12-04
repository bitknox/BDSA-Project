namespace QueueSafe.Shared
{
    public class StoreListDTO
    {
        public int Id { get; set; }
        
        public string Name { get; set; }

        public int Capacity { get; set; }

        public string Address { get; set; }

        public string Image { get; set; }

        public override string ToString() => $"{Name} {Address}";
    }
}