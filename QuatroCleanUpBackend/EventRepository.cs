using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
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

        /// <summary>
        /// I have downloaded the Microsoft.Data.SqlClient package, because it is the
        /// newer version. https://blog.danskingdom.com/Updating-from-System-Data-SqlClient-to-Microsoft-Data-SqlClient/
        /// In this metho I need to create a new event object, add it to the database
        /// and to give it an EventId which increments, with every new creation of events.
        /// </summary>
        /// <param name="newEvent"></param>
        /// <returns>An Event object</returns>
        public Event CreateEventAsync(Event newEvent)
        {
            Event createEvent = newEvent;


            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string SQLQuery = @"INSERT INTO Events ()
                                    VALUES        ";
            }
            return createEvent;

        }


    }
}
