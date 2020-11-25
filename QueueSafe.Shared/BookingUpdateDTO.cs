using System;
using System.ComponentModel.DataAnnotations;

namespace QueueSafe.Shared
{
    public class BookingUpdateDTO
    {
        public BookingState State { get; set; }

        [Required]
        [StringLength(128)]
        public string Token { get; set; }
    }
}