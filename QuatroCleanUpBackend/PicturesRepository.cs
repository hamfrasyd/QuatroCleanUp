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


        public async Task<Picture> AddAsync(Picture picture)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))

                {
                    string query = "INSERT INTO Pictures (EventId, PictureData, Description) VALUES (@EventId, @PictureData, @Description)";

                    var command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@EventId", picture.EventId);
                    command.Parameters.AddWithValue("@PictureData", picture.PictureData);
                    command.Parameters.AddWithValue("@Description", picture.Description);

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();

                }
                return picture;
            }
            catch(Exception ex)
            {
                if (ex.InnerException != null)
                {
                    throw new Exception(ex.InnerException.Message);
                }
                throw new Exception(ex.Message);
            }

        }

        public async Task<List<Picture>> GetAllAsync()
        {
            try
            {
                //initialisere en tom liste 
                var pictureList = new List<Picture>();

                //Opretter forbindelse til databasen 
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string query = "SELECT PictureId, EventId, PictureData, Description FROM Pictures";

                    SqlCommand command = new SqlCommand(query, connection);

                    await connection.OpenAsync();
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
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
            catch(Exception ex)
            {
                if(ex.InnerException != null)
                {
                    throw new Exception(ex.InnerException.Message);
                }
                throw new Exception(ex.Message);
            }

        }

        public async Task<Picture> GetByIdAsync(int id)
        {
            try
            {
                //opretter forbindlese 
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string query = "SELECT PictureId, EventId, PictureData, Description FROM Pictures WHERE PictureId = @PictureId";
                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@PictureId", id);


                    await connection.OpenAsync();
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
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
            catch(Exception ex)
            {
                if (ex.InnerException != null)
                {
                    throw new Exception(ex.InnerException.Message);
                }
                throw new Exception(ex.Message);
            }
            
        }

        public async Task<Picture> DeleteAsync(int id)
        {
            try
            {
                Picture pictures = await GetByIdAsync(id);
                if (pictures != null)
                {
                    //opretter forbindelse 
                    using (SqlConnection connection = new SqlConnection(_connectionString))
                    {
                        string query = "DELETE FROM Pictures WHERE PictureId = @PictureId";
                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@PictureId", id);

                        await connection.OpenAsync();
                        await command.ExecuteNonQueryAsync();
                    }
                }
                else
                {
                    throw new Exception($"picture with ID {id} does not exist.");
                }
                return pictures;
            }
            catch(Exception ex)
            {
                if (ex.InnerException != null)
                {
                    throw new Exception(ex.InnerException.Message);
                }
                throw new Exception(ex.Message);
            }
        }
        
    }
}
