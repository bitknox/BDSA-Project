using System;
using System.ComponentModel.DataAnnotations;

namespace QueueSafe.Entities
{
    public class Booking
    {   
        [Required]
        public virtual Store Store { get; set; }

        public int StoreId { get; set; }
    
        [Required]
        public DateTime TimeStamp { get; set; }
        
        [Required]
        [StringLength(128)]
        public string Token { get; set; } 
    }
}
