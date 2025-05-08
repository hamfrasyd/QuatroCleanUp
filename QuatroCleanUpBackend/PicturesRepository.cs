using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuatroCleanUpBackend
{
    public class PicturesRepository
    {
        private readonly string _connectionString;

        public PicturesRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("QuatroCleanUpDb");
        }


        public Pictures Add(Pictures picture)
        {
            using (var connection = new SqlConnection(_connectionString))

            {
                connection.Open();
                var command = new SqlCommand("INSERT INTO Pictures (EventId, PictureDate, Description) OUTPUT Inserted.PictureId VALUES (@EventId, @PictureDate, @Description); SELECT SCOPE_IDENTITY();", connection);

                command.Parameters.AddWithValue("@EventId", picture.EventId);
                command.Parameters.AddWithValue("@PictureDate", picture.PictureData);
                command.Parameters.AddWithValue("@Description", picture.Description);

                picture.PictureId = (int)command.ExecuteScalar();

            }
            return picture;
        }
    }
}
