using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;


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
            using (SqlConnection connection = new SqlConnection(_connectionString))

            {
                string query = "INSERT INTO Pictures (EventId, PictureDate, Description) VALUES (@EventId, @PictureDate, @Description)";
                
                var command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@EventId", picture.EventId);
                command.Parameters.AddWithValue("@PictureDate", picture.PictureData);
                command.Parameters.AddWithValue("@Description", picture.Description);

                connection.Open();   
                command.ExecuteNonQuery();

            }
            return picture;
        }

        public List<Pictures> GetAll()
        {
            //initialisere en tom liste 
            var pictureList = new List<Pictures>();

            //Opretter forbindelse til databasen 
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT PictureId, EventId, PictureDate, Description FROM Pictures";

                var command = new SqlCommand(query, connection);

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Pictures picture = new Pictures
                        {
                            PictureId = (int)reader["PictureId"],
                            EventId = (int)reader["EventId"],
                            PictureData = (byte[])reader["PictureData"],
                            Description = (string)reader["Description"]
                        };

                        pictureList.Add(picture);
                    }
                }
            }
            return pictureList;

        }

        public Pictures GetById(int id)
        {

        }

        public Pictures Delete(int id)
        {

        }
    }
}
