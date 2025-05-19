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

        //CRUDs

        /// <summary>
        /// 
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
                    string SqlQuery = @"INSERT INTO EventAttendances ";
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
        public async Task<EventAttendance> GetByIdAsync(int it)
        {
            try
            {

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
