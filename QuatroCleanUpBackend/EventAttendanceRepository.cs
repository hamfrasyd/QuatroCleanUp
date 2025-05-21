using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using PlayFab.AdminModels;

namespace QuatroCleanUpBackend
{
    class EventAttendanceRepository
    {
        private readonly string _connectionString;

        public EventAttendanceRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("QuatroCleanUpDb");   
        }

        //CRUDs - skal vi have delete though? Du sletter jo en eventAttendance ved at slette en af dens primære nøgler.

        /// <summary>
        /// Creates an EventAttendance based on CreatedDate, EventId and UserId.
        /// </summary>
        /// <param name="eventAttendance"></param>
        /// <returns>EventAttendance</returns>
        /// <exception cref="Exception"></exception>
        public async Task<EventAttendance> CreateAsync(EventAttendance eventAttendance)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string SqlQuery = @"INSERT INTO EventAttendances (EventId, UserId, CreatedDate)
                                        VALUES(@EventId, @UserId, @CreatedDate);";

                    SqlCommand command = new SqlCommand(SqlQuery, connection);
                    command.Parameters.AddWithValue("@EventId", eventAttendance.SomeEvent.EventId);
                    command.Parameters.AddWithValue("@UserId", eventAttendance.SomeUser.UserId);
                    command.Parameters.AddWithValue("@CreatedDate", eventAttendance.CreatedDate);

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                    return eventAttendance;
                }
            }
            catch(DbException ex)
            {
                Console.Error.WriteLine($"{ex.Message}");
                throw new Exception(ex.Message);
            }
            catch(Exception ex)
            {
                Console.Error.WriteLine($"{ex.Message}");
                throw new Exception(ex.Message);
            }



        }

        /// <summary>
        /// Get a specific EventAttendance and some data from the User and Event table also. Perhaps should be all of it.
        /// </summary>
        /// <param name="it"></param>
        /// <returns>EventAttendance</returns>
        /// <exception cref="Exception"></exception>
        //public async Task<EventAttendance> GetByIdAsync(int eventId, int userId)
        //{
        //    try
        //    {
        //        using (SqlConnection connection = new SqlConnection(_connectionString))
        //        {
        //            string SqlQuery = @"SELECT 
        //                    eventAttendance.EventId, eventAttendance.UserId, eventAttendance.CheckIn, eventAttendance.CreatedDate," +
        //                "   event.EventId AS Event_EventId, event.Title, event.Description, event.StartTime, event.EndTime, " +
        //                "   event.FamilyFriendly, event.Participants, event.TrashCollected" +
        //                "   event.StatusId, event.LocationId" +
        //                "   user.UserId AS User_UserId, user.RoleId, user.Name, user.Email, user.CreatedDate" +
        //                "   FROM EventAttendances eventAttendance" +
        //                "   JOIN Events event ON eventAttendance = event.EventId" +
        //                "   JOIN Users user ON eventAttendance = user.UserId ";



        //            List<EventAttendance> attandances = new List<EventAttendance>();

        //            SqlCommand command = new SqlCommand(SqlQuery, connection);
        //            command.Parameters.AddWithValue("@EventId", eventId);
        //            command.Parameters.AddWithValue("@UserId", userId);

        //            await connection.OpenAsync();
        //            using (SqlDataReader reader = await command.ExecuteReaderAsync())
        //            {
        //                if (await reader.ReadAsync())
        //                {
        //                    return new EventAttendance
        //                    {
        //                        SomeEvent = (Event)reader["EventId"],
        //                        SomeUser = (User)reader["UserId"],
        //                        CreatedDate = (DateTime)reader["CreatedDate"],
        //                        CheckIn = (bool)reader["CheckIn"]
        //                        //Skal vi have flere data med her til PlayFab? TrashCollected?
        //                    };
        //                }
        //                else
        //                {
        //                    throw new KeyNotFoundException($"Event with Id {eventId} does not exist.");
        //                }
        //            }
        //        }
        //    }
        //    catch (KeyNotFoundException ex)
        //    {
        //        Console.Error.WriteLine($"{ex.Message}");
        //        throw new Exception(ex.Message);
        //    }
        //    catch (DbException ex)
        //    {
        //        Console.Error.WriteLine($"{ex.Message}");
        //        throw new Exception(ex.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.Error.WriteLine($"{ex.Message}");
        //        throw new Exception(ex.Message);
        //    }
        //}

        /// <summary>
        /// GetAllAsync fetches all data in the EventAttendance table and therefore in the subsequent Event and User tables
        /// </summary>
        /// <returns>EventAttendance</returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<EventAttendance>> GetAllAsync()
        {
            List<EventAttendance> attendanceList = new List<EventAttendance>();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string SqlQuery = @"SELECT 
                            eventAttendance.EventId, eventAttendance.UserId, eventAttendance.CheckIn, eventAttendance.CreatedDate," +
                        "   event.EventId AS Event_EventId, event.Title, event.Description, event.StartTime, event.EndTime, " +
                        "   event.FamilyFriendly, event.Participants, event.TrashCollected" +
                        "   event.StatusId, event.LocationId" +
                        "   user.UserId AS User_UserId, user.RoleId, user.Name, user.Email, user.CreatedDate" +
                        "   FROM EventAttendances eventAttendance" +
                        "   JOIN Events event ON eventAttendance = event.EventId" +
                        "   JOIN Users user ON eventAttendance = user.UserId ";

                    SqlCommand command = new SqlCommand(SqlQuery, connection);

                    await connection.OpenAsync();

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while(await reader.ReadAsync())
                        {
                            Event attendedEvent = new Event
                            {
                                EventId = (int)reader["Event_EventId"],
                                Title = (string)reader["Title"],
                                Description = (string)reader["Description"],
                                StartTime = (DateTime)reader["StartTime"],
                                EndTime = (DateTime)reader["EndTime"],
                                FamilyFriendly = (bool)reader["FamilyFriendly"],
                                Participants = reader["Participants"] != DBNull.Value ? (int)reader["Participants"] : 0,
                                TrashCollected = (decimal)reader["Event_TrashCollected"],
                                StatusId = (int)reader["StatusId"],
                                LocationId = (int)reader["LocationId"]
                            };

                            User attendedUser = new User
                            {
                                UserId = (int)reader["User_UserId"],
                                Name = (string)reader["Name"],
                                RoleId = (int)reader["RoleId"],
                                Email = (string)reader["Email"],
                                CreatedDate = (DateTime)reader["CreatedDate"],
                                AvatarPic = (byte[])reader["AvatarPic"]
                            };

                            EventAttendance eventAttendance = new EventAttendance
                            {
                                SomeEvent = attendedEvent,
                                SomeUser = attendedUser,
                                CheckIn = (bool)reader["CheckIn"],
                                CreatedDate = (DateTime)reader["CreatedDate"],
                            };

                            attendanceList.Add(eventAttendance);
                        }
                    }                
                }
                return attendanceList;
            }
            catch (DbException ex)
            {
                Console.Error.WriteLine($"{ex.Message}");
                throw new Exception(ex.Message);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"{ex.Message}");
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Update EventAttendance, though not CreatedDate.
        /// </summary>
        /// <param name="eventAttendanceUpdate"></param>
        /// <returns>EventAttendance</returns>
        /// <exception cref="Exception"></exception>
        public async Task<EventAttendance> UpdateAsync(EventAttendance eventAttendanceUpdate)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string SqlQuery = @"UPDATE EventAttendance" +
                                        "SET CreatedDate = @CreatedDate" +
                                        "WHERE EventId = @EventId AND UserId = @UserId";

                    SqlCommand command = new SqlCommand(SqlQuery, connection);
                    command.Parameters.AddWithValue(@"EventId", eventAttendanceUpdate.SomeEvent.EventId);
                    command.Parameters.AddWithValue(@"", eventAttendanceUpdate.SomeUser.UserId);
                    command.Parameters.AddWithValue(@"CreatedDate", eventAttendanceUpdate.CheckIn);

                    await connection.OpenAsync();
                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    if (rowsAffected == 0)
                    {
                        throw new ArgumentNullException("Failed to update the event, as EventAttendance was not found");
                    }
                    return eventAttendanceUpdate;
                }
            }
            catch(DbException ex)
            {
                Console.Error.WriteLine($"{ex.Message}");
                throw new Exception(ex.Message);
            }
            catch(Exception ex)
            {
                Console.Error.WriteLine($"{ex.Message}");
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Searches for the correct EventAttendance, based on the SomeUser.UserId and SomeEvent.EventId and then deletes it.
        /// </summary>
        /// <param name="eventAttendanceDelete"></param>
        /// <returns>EventAttendance</returns>
        /// <exception cref="Exception"></exception>
        public async Task<EventAttendance> DeleteAsync(EventAttendance eventAttendanceDelete)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string SqlQuery = @"DELTE FROM EventAttendances WHERE EventId = @EventId AND UserId = @UserId";

                    SqlCommand command = new SqlCommand(SqlQuery, connection);
                    command.Parameters.AddWithValue("@EventId", eventAttendanceDelete.SomeEvent.EventId);
                    command.Parameters.AddWithValue("@UserId", eventAttendanceDelete.SomeUser.UserId);

                    await connection.OpenAsync();
                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    if(rowsAffected == 0)
                    {
                        throw new Exception("The EventAttendance could not be found.");
                    }
                    return eventAttendanceDelete;
                }
            }
            catch(DBConcurrencyException ex)
            {
                Console.Error.WriteLine($"Database Error: {ex.Message}");
                throw new Exception(ex.Message);
            }
            catch(Exception ex)
            {
                if (ex.InnerException != null)
                {
                    Console.Error.WriteLine($"Some error occurred: {ex.InnerException.Message}");
                    throw new Exception(ex.InnerException.Message);
                }
                Console.Error.WriteLine($"Some error occurred: {ex.Message}");
                throw new Exception(ex.Message);
            }
        }

    }
}
