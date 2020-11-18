using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace QueueSafe.Entities
{
    public class Store
    {        
        public int Id { get; set; }

        [Required]
        public int Capacity { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; } 

        public virtual ICollection<Booking> Bookings { get; set; }
    }
}