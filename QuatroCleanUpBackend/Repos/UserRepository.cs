using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using QuatroCleanUpBackend.Models;
using QuatroCleanUpBackend.Validators;

namespace QuatroCleanUpBackend.Repos
{
    public class UserRepository
    {
        private readonly string _connectionString;

        /// <summary>
        /// Installed the following package to use the IConfiguration framwork 
        /// microsoft extension configuration
        /// </summary>
        /// <param name="configuration"></param>
        public UserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("QuatroCleanUpDb");

        }

        /// <summary>
        /// Method to create a new user, using the SqlConnecntion & SqlCommand and return the user upon succes 
        /// </summary>
        /// <param name="user"></param>
        public async Task<User> CreateUserAsync(User newUser)
        {
            ValidateMethodForUser.ValidateUser(newUser);

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    string SqlQuery = @"INSERT INTO Users (Name, Email, PasswordHash, RoleId, CreatedDate, AvatarPictureId)
                                    VALUES (@Name, @Email, @PasswordHash, @RoleId, @CreatedDate, @AvatarPictureId); SELECT SCOPE_IDENTITY()";

                    SqlCommand command = new SqlCommand(SqlQuery, connection);
                    command.Parameters.AddWithValue("@Name", newUser.Name);
                    command.Parameters.AddWithValue("@Email", newUser.Email);
                    command.Parameters.AddWithValue("@PasswordHash", newUser.Password);
                    command.Parameters.AddWithValue("@RoleId", newUser.RoleId);
                    command.Parameters.AddWithValue("@CreatedDate", newUser.CreatedDate);
                    command.Parameters.AddWithValue("@AvatarPictureId", newUser.AvatarPictureId);

                    await connection.OpenAsync();
                    var fetchId = await command.ExecuteScalarAsync();

                    newUser.UserId = Convert.ToInt32(fetchId);
                    return newUser;
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Sql Error: {ex.Message}");
                    throw new Exception("Fejl i Create " + ex.Message);
                }
            }

        }

        /// <summary>
        /// Method for getting all user obejct from the Users table i SQL. 
        /// Using ExecuteReaderAsync instead of ExecuteScalarAsync, because we need alle useres. Et returns a SqlDataReader, which reads the whole table, colums and rows. 
        /// </summary>
        /// <returns>Returns a list of Useres</returns>
        public async Task<List<User>> GetAllAsync()
        {
            List<User> userList = new List<User>();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string SqlQuery = "SELECT * FROM Users";

                    SqlCommand command = new SqlCommand(SqlQuery, connection);

                    await connection.OpenAsync();

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            User newUser = new User
                            {
                                UserId = (int)reader["UserId"],
                                Name = (string)reader["Name"],
                                Email = (string)reader["Email"],
                                Password = (string)reader["PasswordHash"],
                                RoleId = (int)reader["RoleId"],
                                CreatedDate = (DateTime)reader["CreatedDate"],
                                AvatarPictureId = (int)reader["AvatarPictureId"]
                            };
                            userList.Add(newUser);
                        }
                    }
                }
                return userList;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"{ex.Message}");
                throw new Exception("Fejl i get all " + ex.Message);

            }
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string SqlQuery = "SELECT * FROM Users WHERE UserId = @UserId";

                    SqlCommand command = new SqlCommand(SqlQuery, connection);
                    command.Parameters.AddWithValue("@UserId", userId);

                    await connection.OpenAsync();
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new User
                            {
                                UserId = (int)reader["UserId"],
                                Name = (string)reader["Name"],
                                Email = (string)reader["Email"],
                                Password = (string)reader["PasswordHash"],
                                RoleId = (int)reader["RoleId"],
                                CreatedDate = (DateTime)reader["CreatedDate"],
                                AvatarPictureId = (int)reader["AvatarPictureId"],
                            };

                        }
                        else
                        {
                            throw new InvalidOperationException($"User with Id {userId} does not exist.");
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Fejl i get by id: {ex.Message}");
                throw;
            }


        }

        public async Task<User> UpdateUserAsync(User userUpdate)
        {

            if (userUpdate == null)
            {
                throw new ArgumentNullException("User not found. Please create a new user");
            }
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string SqlQuery = @"UPDATE Users SET
                                     Name = @Name,
                                     Email = @Email,
                                     PasswordHash = @PasswordHash,
                                     AvatarPictureId = @AvatarPictureId
                                  WHERE
                                     UserId = @UserId";
                    SqlCommand command = new SqlCommand(SqlQuery, connection);
                    command.Parameters.AddWithValue("@UserId", userUpdate.UserId);
                    command.Parameters.AddWithValue("@Name", userUpdate.Name);
                    command.Parameters.AddWithValue("@Email", userUpdate.Email);
                    command.Parameters.AddWithValue("@PasswordHash", userUpdate.Password);
                    command.Parameters.AddWithValue("@AvatarPictureId", userUpdate.AvatarPictureId);

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();

                }
                return userUpdate;

            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Sql Error: {ex.Message}");
                throw new Exception("Fejl i update " + ex.Message);

            }

        }

        public async Task<User> DeleteUserAsync(int userId)
        {
            var getUser = await GetUserByIdAsync(userId);
            if (getUser == null)
            {
                throw new ArgumentNullException("This user does not exist");
            }
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string SqlQuery = "DeLETE FROM USers WHERE UserId = @UserId";

                    SqlCommand command = new SqlCommand(SqlQuery, connection);
                    command.Parameters.AddWithValue("@UserId", userId);

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                    return getUser;
                }
            }
            catch (SqlException ex)
            {
                Console.Error.WriteLine($"Sql Error: {ex.Message}");
                throw new Exception(ex.Message);
            }
            catch (Exception ex)
            {
                if (ex.InnerException.Message != null)
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
