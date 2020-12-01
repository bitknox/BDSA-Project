using System;
using System.ComponentModel.DataAnnotations;

namespace QueueSafe.Shared
{
    public class StoreDetailsDTO
    {
        [Required]
        [StringLength(20)]
        public string Name { get; set; }

        public int Capacity { get; set; }

        public string Address { get; set; }

        public string Image { get; set; }
    }
}