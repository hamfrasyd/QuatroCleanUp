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


        public Picture Add(Picture picture)
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

        public List<Picture> GetAll()
        {
            //initialisere en tom liste 
            var pictureList = new List<Picture>();

            //Opretter forbindelse til databasen 
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT PictureId, EventId, PictureDate, Description FROM Pictures";

                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Picture picture = new Picture
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

        public Picture GetById(int id)
        {
            //opretter forbindlese 
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT PictureId, EventId, PictureDate, Description FROM Pictures WHERE PictureId = @PictureId";
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@PictureId", id);


                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Picture
                        {
                            PictureId = (int)reader["PictureId"],
                            EventId = (int)reader["EventId"],
                            PictureData = (byte[])reader["PictureData"],
                            Description = (string)reader["Description"]
                        };
                    }
                    else
                    {
                        throw new Exception($"picture with ID {id} does not exist.");
                    }
                }


            }
            
        }

        public Picture Delete(int id)
        {

            Picture pictures = GetById(id);
            if (pictures != null)
            {
                //opretter forbindelse 
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string query = "DELETE FROM Pictures WHERE PictureId = @PictureId";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@PictureId", id);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            else
            {
                throw new Exception($"picture with ID {id} does not exist.");
            }
            return pictures;
        }
        
    }
}
