namespace QueueSafe.Shared
{
    public class BookingUpdateDTO : BookingCreateDTO
    {
        public BookingState State { get; set; }
    }
}