using System;
using System.ComponentModel.DataAnnotations;

namespace QueueSafe.Entities
{
    public class Booking
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
}
