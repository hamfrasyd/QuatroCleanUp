
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace QuatroCleanUpBackend
{
    public class UserRepository
    {
        private readonly string _connectionString;

        public UserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("QuatroCleanUpDb");

        }

        public void CreateUser(User user)
        {
            ValidateMethodForUser.ValidateUser(user);
            using( var connection = new SqlConnection(_connectionString))
            {
                var query = "INSERT INTO Users (Name, Email, Password) VALUES (@Name, @Email, @Password)";
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Name", user.Name);
                command.Parameters.AddWithValue("@Email", user.Email);
                command.Parameters.AddWithValue("@Password", user.Password);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public List<User> GetAll()
        {
            var users = new List<User>();

            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "SELCET * FROM Users";
                var command = new SqlCommand(query, connection);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var user = new User
                        {
                            UserId = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Email = reader.GetString(2),
                            Password = reader.GetString(3)
                        };
                        users.Add(user);

                    }
                }
            }
            return users;
        }

        public User GetUserById(int userId)
        {
           using(var connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT * FROM Users WHERE UserId = @UserId";
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserId", userId);

                connection.Open();

                using(var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new User
                        {
                            UserId = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Email = reader.GetString(2),
                            Password = reader.GetString(3)
                        };
                    }
                }
            }
                return null;            
        }

        public User UpdateUser(User user)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"UPDATE Users
                              SET Name = @Name, Email = @Email, Password = @Password
                              WHERE UserId = @UserId";
                        
                var command = new SqlCommand(query,connection);
                command.Parameters.AddWithValue("@UserId", user.UserId);
                command.Parameters.AddWithValue("@Name", user.Name);
                command.Parameters.AddWithValue("@Email", user.Email);
                command.Parameters.AddWithValue("@Password", user.Password);

                connection.Open();
                command.ExecuteNonQuery();
                
            }
            return user;
        }

        public User DeleteUser(User user)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "DELETE FROM Users WHERE UserId = @UserId";
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserId", user.UserId);

                connection.Open();
            }
            return null;

        }

    }
}
