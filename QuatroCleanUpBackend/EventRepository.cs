using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace QuatroCleanUpBackend
{
    public class EventRepository
    {
        private readonly string _connectionString;

        /// <summary>
        /// Installed the following package to use the IConfiguration framework
        /// microsoft extension configuration.
        /// </summary>
        /// <param name="configuration"></param>
        public EventRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("QuatroCleanUpDb");

        }

        public Event CreateEventAsync(Event newEvent)
        {
            Event createEvent = newEvent;

            return createEvent;

        }


    }
}
