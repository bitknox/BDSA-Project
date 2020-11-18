using System;
using System.ComponentModel.DataAnnotations;

namespace QueueSafe.Shared
{
    public class BookingCreateDTO
    {
        private DateTime _TimeStamp;

        [Required]
        [StringLength(50)]
        public string StoreName { get; set; }
    
        [Required]
        public DateTime TimeStamp {
            get => _TimeStamp;
            set => _TimeStamp = DateTime.Now;
        }
        
        [Required]
        [StringLength(128)]
        public string Token { get; set; } 
    }
}
