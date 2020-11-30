using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace QueueSafe.Entities
{
    public class City
    {
        public int Postal { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Address> Addresses { get; set; }
    }
}