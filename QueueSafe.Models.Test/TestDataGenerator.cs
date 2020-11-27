using System.Collections.Generic;
using System;
using QueueSafe.Entities;
using static QueueSafe.Entities.BookingState;

namespace QueueSafe.Models.Test
{
    public static class TestDataGenerator
    {
        public static void GenerateTestData(this QueueSafeContext context)
        {
            var GaldalfsButtHashing = new Store
            {
                Name = "GandalfsButtHashing",
                Capacity = 50,
                Bookings = new List<Booking>
                {
                    new Booking { TimeStamp = DateTime.Now, State = Active, Token = "goodtoken1" },
                    new Booking { TimeStamp = DateTime.Now, State = Pending, Token = "goodtoken2" }
                }
            };

            var ElMinidik = new Store
            {
                Name = "ElMinidik",
                Capacity = 51,
                Bookings = new List<Booking>
                {
                    new Booking { TimeStamp = DateTime.Now, State = Active, Token = "goodtoken3" },
                    new Booking { TimeStamp = DateTime.Now, State = Pending, Token = "goodtoken4" },
                    new Booking { TimeStamp = DateTime.Now, State = Canceled, Token = "goodtoken5" }
                }
            };

            context.Store.AddRange(GaldalfsButtHashing, ElMinidik);
            context.SaveChanges();
        }
    }
}