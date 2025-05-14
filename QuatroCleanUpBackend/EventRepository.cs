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
        /// In this SqlQuery we end it with a request for Identity, in this case the EventId.
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
                string SqlQuery = @"INSERT INTO Events (Title, Description, StartTime, EndTime, FamilyFriendly, Participants, PictureId, TrashCollected, StatusId, LocationId)
                                    VALUES (@Title, @Description, @StartTime, @EndTime, @FamilyFriendly, @Participants, @PictureId, @TrashCollected, @StatusId, @LocationId); SELECT SCOPE_IDENTITY()";

                SqlCommand command = new SqlCommand(SqlQuery, connection);
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

        /// <summary>
        /// In this method we get all event objects from the Events table in the SQL.
        /// We use the ExecuteReaderAsync instead of ExecuteScalarAsync, because we don't need to query, but
        /// rather need to fetch all the events we can. ExecuteReader returns a SqlDataReader, which reads the table
        /// columns of the table, row by row.
        /// </summary>
        /// <returns>A list of Events</returns>
        public async Task<List<Event>> GetAllAsync()
        {
            List<Event> eventList = new List<Event>();

            using (SqlConnection connection = new SqlConnection())
            {
                string SqlQuery = "SELECT * FROM Events";

                SqlCommand command = new SqlCommand(SqlQuery, connection);

                await connection.OpenAsync();

                using (SqlDataReader reader = await command.ExecuteReaderAsync()) 
            //ExecuteReader executes the query and returns a SqlDataReader to read the table row by row. 
                {
                    while (await reader.ReadAsync())
                    {
                        Event newEvent = new Event
                        {
                            EventId = (int)reader["EventId"],
                            Title = (string)reader["Title"],
                            Description = (string)reader["Description"],
                            StartTime = (DateTime)reader["StartTime"],
                            EndTime = (DateTime)reader["EndTime"],
                            FamilyFriendly = (bool)reader["FamilyFriendly"],
                            Participants = (int)reader["Participants"],
                            PictureId = (byte[])reader["PictureId"],
                            TrashCollected = (decimal)reader["TrashCollected"],
                            StatusId = (int)reader["StatusId"],
                            LocationId = (int)reader["LocationId"]
                        };
                        eventList.Add(newEvent);
                    }
                }
                return eventList;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Event> GetByIdAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string SqlQuery = "SELECT * FROM Events WHERE EventId = @EventId";

                SqlCommand command = new SqlCommand(SqlQuery, connection);
                command.Parameters.AddWithValue("@EventId", id);

                await connection.OpenAsync();
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new Event
                        {
                            EventId = (int)reader["EventId"],
                            Title = (string)reader["Title"],
                            Description = (string)reader["Description"],
                            StartTime = (DateTime)reader["StartTime"],
                            EndTime = (DateTime)reader["EndTime"],
                            FamilyFriendly = (bool)reader["FamilyFriendly"],
                            Participants = (int)reader["Participants"],
                            PictureId = (byte[])reader["PictureId"],
                            TrashCollected = (decimal)reader["TrashCollected"],
                            StatusId = (int)reader["StatusId"],
                            LocationId = (int)reader["LocationId"]

                        };
                    }
                    else
                    {
                        throw new InvalidOperationException($"Event with Id {id} does not exist.");
                    }
                }


            }


        }

        public async Task<Event> UpdateEventAsync(Event eventUpdate)
        {

            if(eventUpdate == null)
            {
                throw new ArgumentNullException(nameof(eventUpdate), "You need to input information if you want to update the event.");
            } 
            using(SqlConnection connection = new SqlConnection(_connectionString))
            {
                string SqlQuery = @"UPDATE Events SET 
                                    Title = @Title, 
                                    Description = @Description,
                                    StartTime = @StartTime,
                                    EndTime = @EndTime,
                                    FamilyFriendly = @FamilyFriendly,
                                    Participants = @Participants,
                                    PictureId = @PictureId,
                                    TrashCollected = @TrashCollected,
                                    StatusId = @StatusId,
                                    LocationId = @LocationId
                                WHERE
                                    EventId = @EventId";


                SqlCommand command = new SqlCommand(SqlQuery, connection);
                command.Parameters.AddWithValue("@Title", eventUpdate.Title);
                command.Parameters.AddWithValue("@Description", eventUpdate.Description);
                command.Parameters.AddWithValue("@StartTime", eventUpdate.StartTime);
                command.Parameters.AddWithValue("@EndTime", eventUpdate.EndTime);
                command.Parameters.AddWithValue("@FamilyFriendly", eventUpdate.FamilyFriendly);
                command.Parameters.AddWithValue("@Participants", eventUpdate.Participants);
                command.Parameters.AddWithValue("@PictureId", eventUpdate.PictureId);
                command.Parameters.AddWithValue("@TrashCollected", eventUpdate.TrashCollected);
                command.Parameters.AddWithValue("@StatusId", eventUpdate.StatusId);
                command.Parameters.AddWithValue("@LocationId", eventUpdate.LocationId);


                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            
            }


        }

        public async Task<Event> DeleteEventAsync()
        {

        }
    }
}
