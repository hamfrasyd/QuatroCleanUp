using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

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
        /// <returns></returns>
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
        /// 
        /// </summary>
        /// <param name="it"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<EventAttendance> GetByIdAsync(int eventId, int userId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string SqlQuery = "SELECT eventAttendance.EventId, eventAttendance.UserId, eventAttendance.CheckIn, eventAttendance.CreatedDate," +
                        "               event.Title, user.Name" +
                        "               FROM EventAttendances eventAttendance" +
                        "               JOIN Events event ON eventAttendance = event.EventId" +
                        "               JOIN Users user ON eventAttendance = user.UserId ";

                    List<EventAttendance> attandances = new List<EventAttendance>();

                    SqlCommand command = new SqlCommand(SqlQuery, connection);
                    command.Parameters.AddWithValue("@EventId", eventId);
                    command.Parameters.AddWithValue("@UserId", userId);

                    await connection.OpenAsync();
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new EventAttendance
                            {
                                SomeEvent = (Event)reader["EventId"],
                                SomeUser = (User)reader["UserId"],
                                CreatedDate = (DateTime)reader["CreatedDate"],
                                CheckIn = (bool)reader["CheckIn"]
                                //Skal vi have flere data med her til PlayFab? TrashCollected?
                            };
                        }
                        else
                        {
                            throw new KeyNotFoundException($"Event with Id {id} does not exist.");
                        }
                    }
                }
            }
            catch (KeyNotFoundException ex)
            {
                Console.Error.WriteLine($"{ex.Message}");
                throw new Exception(ex.Message);
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
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<EventAttendance>> GetAllAsync()
        {
            try
            {

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

        public async Task<EventAttendance> UpdateAsync(EventAttendance eventAttendanceUpdate)
        {
            try
            {

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
        {

        }

    }
}
