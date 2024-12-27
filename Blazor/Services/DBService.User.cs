using Domain_Models;
using Npgsql;

namespace Blazor.Services
{
    public partial class DBService
    {
        public async Task<List<User>> GetAllUsersAsync()
        {
            try
            {
                Console.WriteLine("Getting all Users");
                await using var connection = new NpgsqlConnection(_connectionString);
                await connection.OpenAsync();
                
                string sql = @"SELECT id, username, email, password_hash, created_at, updated_at, last_login, is_active, first_name, last_name 
                                FROM ""User""";
                Console.WriteLine(sql);
                
                await using var command = new NpgsqlCommand(sql, connection);
                await using var reader = await command.ExecuteReaderAsync();
                
                List<User> users = new List<User>();
                while (await reader.ReadAsync())
                {
                    users.Add(MapSQLToUser(reader));
                }
                return users;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database fejl: {ex.Message}");
                throw;
            }
        }

        public async Task<User> GetUserByIdAsync(string id)
        {
            try
            {
                string sql = @"SELECT * FROM ""User"" 
                                WHERE id = @id";
                using var connection = new NpgsqlConnection(_connectionString);
                using var command = new NpgsqlCommand(sql, connection);
                command.Parameters.AddWithValue("@id", id);
                await connection.OpenAsync();
                using var reader = await command.ExecuteReaderAsync();
                
                if (!await reader.ReadAsync())
                    return null; 
                    
                return MapSQLToUser(reader);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<User> PostUserAsync(User user)
        {
            try
            {
                string sql = @"INSERT INTO ""User"" 
                (id, username, email, password_hash, created_at, updated_at, last_login, is_active, first_name, last_name) 
                VALUES (@id, @username, @email, @password_hash, @created_at, @updated_at, @last_login, @is_active, @first_name, @last_name)";
                using var connection = new NpgsqlConnection(_connectionString);
                using var command = new NpgsqlCommand(sql, connection);
                command.Parameters.AddWithValue("@id", user.Id ?? Guid.NewGuid().ToString());
                command.Parameters.AddWithValue("@username", user.Username ?? string.Empty);
                command.Parameters.AddWithValue("@email", user.Email ?? string.Empty);
                command.Parameters.AddWithValue("@password_hash", user.PasswordHash ?? string.Empty);
                command.Parameters.AddWithValue("@created_at", DateTime.UtcNow);
                command.Parameters.AddWithValue("@updated_at", DateTime.UtcNow);
                command.Parameters.AddWithValue("@last_login", DateTime.UtcNow);
                command.Parameters.AddWithValue("@is_active", false);
                command.Parameters.AddWithValue("@first_name", user.FirstName ?? string.Empty);
                command.Parameters.AddWithValue("@last_name", user.LastName ?? string.Empty); 
                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<User> PutUserAsync(User user)
        {
            try
            {
                string sql = @"UPDATE ""User"" 
                SET username = @username, 
                email = @email, 
                password_hash = @password_hash, 
                updated_at = @updated_at, 
                last_login = @last_login, 
                is_active = @is_active, 
                first_name = @first_name, 
                last_name = @last_name 
                WHERE id = @id";
                
                using var connection = new NpgsqlConnection(_connectionString);
                using var command = new NpgsqlCommand(sql, connection); 
                command.Parameters.AddWithValue("@id", user.Id);
                command.Parameters.AddWithValue("@username", user.Username);
                command.Parameters.AddWithValue("@email", user.Email);
                command.Parameters.AddWithValue("@password_hash", user.PasswordHash);
                command.Parameters.AddWithValue("@updated_at", user.UpdatedAt);
                command.Parameters.AddWithValue("@last_login", user.LastLogin ?? DateTime.UtcNow);
                command.Parameters.AddWithValue("@is_active", user.IsActive);
                command.Parameters.AddWithValue("@first_name", user.FirstName ?? string.Empty);
                command.Parameters.AddWithValue("@last_name", user.LastName ?? string.Empty);
                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task<string> DeleteUserAsync(string id)
        {
            try
            {
                string sql = @"DELETE FROM ""User"" 
                                WHERE id = @id";
                using var connection = new NpgsqlConnection(_connectionString);
                using var command = new NpgsqlCommand(sql, connection);
                command.Parameters.AddWithValue("@id", id);
                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
                return $"User {id} deleted";    
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        
        public User MapSQLToUser(NpgsqlDataReader reader)
        {
            return new User
            {
                Id = reader.GetString(0),
                Username = reader.IsDBNull(1) ? "Dummy" : reader.GetString(1),
                Email = reader.IsDBNull(2) ? "Dummy@dummy.com" : reader.GetString(2),
                PasswordHash = reader.IsDBNull(3) ? "password123" : reader.GetString(3),
                CreatedAt = reader.IsDBNull(4) ? DateTime.MinValue : reader.GetDateTime(4),
                UpdatedAt = reader.IsDBNull(5) ? DateTime.MinValue : reader.GetDateTime(5),
                LastLogin = reader.IsDBNull(6) ? DateTime.MinValue : reader.GetDateTime(6),
                IsActive = reader.IsDBNull(7) ? false : reader.GetBoolean(7),
                FirstName = reader.IsDBNull(8) ? null : reader.GetString(8),
                LastName = reader.IsDBNull(9) ? null : reader.GetString(9)
            };
        }
    }
} 