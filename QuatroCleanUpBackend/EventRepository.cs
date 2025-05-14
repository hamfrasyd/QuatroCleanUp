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
        /// First we create a SqlConnection to our database, using our connectionString saved in appSettings.
        /// Then, we create a string, into which we can put all our information, i.e. an Event object.
        /// In this SQLQuery we end it with a request for Identity, in this case the EventId.
        /// Then we create a new SqlCommand - a datatype used to represent a transact sql-statement.
        /// Into this command we put the information from the new evet object - using the SqlCommand's property, parameters.
        /// Then we finally open a connection to the database.
        /// Finally we execute our SqlCommand object, which ships our information to the database.
        /// </summary>
        /// <param name="newEvent"></param>
        /// <returns>An Event object</returns>
        public async Task<Event> CreateEventAsync(Event newEvent)
        {
            //Event createEvent = newEvent;
            //Validate: positive number, DateTime, NotNull, minimumLengthString, 

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string SQLQuery = @"INSERT INTO Events (Title, Description, StartTime, EndTime, FamilyFriendly, Participants, PictureId, TrashCollected, StatusId, LocationId)
                                    VALUES (@Title, @Description, @StartTime, @EndTime, @FamilyFriendly, @Participants, @PictureId, @TrashCollected, @StatusId, @LocationId); SELECT SCOPE_IDENTITY()";

                SqlCommand command = new SqlCommand(SQLQuery, connection);
                command.Parameters.AddWithValue("@Title", newEvent.Title);
                command.Parameters.AddWithValue("@Description", newEvent.Description);
                command.Parameters.AddWithValue("@StartTime", newEvent.StartTime);
                command.Parameters.AddWithValue("@EndTime", newEvent.EndTime);
                command.Parameters.AddWithValue("@FamilyFriendly", newEvent.FamilyFriendly);
                command.Parameters.AddWithValue("@Participants", newEvent.Participants);
                command.Parameters.AddWithValue("@PictureId", newEvent.PictureId);
                command.Parameters.AddWithValue("@TrashCollected", newEvent.TrashCollected);
                command.Parameters.AddWithValue("@StatusId", newEvent.StatusId);
                command.Parameters.AddWithValue("@LocationId", newEvent.LocationId);

                await connection.OpenAsync();
                var fetchId = await command.ExecuteScalarAsync(); //ExecuteScalarAsync returns object

                newEvent.EventId = Convert.ToInt32(fetchId); //therefore we convert the fetchedId to int.
                return newEvent;
            }         

        }

        public async Task<List<Event>> GetAllAsync()
        {
            List<Event> eventList = new List<Event>();

            using (SqlConnection connection = new SqlConnection())
            {
                string SQLQuery = "SELECT * FROM Events";

                SqlCommand command = new SqlCommand(SQLQuery, connection);

                await connection.OpenAsync();

                using (SqlDataReader reader = await command.ExecuteReaderAsync()) //ExecuteReader executest the query and returns a SqlDataReader to read the table row by row. 
                {
                    while (await reader.ReadAsync())
                    {
                        Event newEvent = new Event()
                        {

                        }

                    }
                }
            }
        }

        public async Task<Event> GetByIdAsync(int id)
        {

        }

        public async Task<Event> UpdateEventAsync(Event eventUpdate)
        {


        }

        public async Task<Event> DeleteEventAsync()
        {

        }
    }
}
