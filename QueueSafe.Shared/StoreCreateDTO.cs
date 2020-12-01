using System;
using System.ComponentModel.DataAnnotations;

namespace QueueSafe.Shared
{
    public class StoreCreateDTO
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public int Capacity { get; set; }

        public string Image { get; set; }

        [Required]
        public AddressDTO Address { get; set; }        
    }
}