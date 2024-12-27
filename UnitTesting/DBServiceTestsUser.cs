using Blazor.Services;
using Domain_Models;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace UnitTesting
{
    public class DBServiceTestsUser
    {
        private readonly DBService _dbService;
        private readonly string _connectionString;
        

        public DBServiceTestsUser()
        {
            // Opsæt configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            // Hent connection string fra appsettings
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _dbService = new DBService(_connectionString);
        }

        [Fact]
        public async Task TestConnection_ShouldReturnTrue()
        {
            // Act
            bool result = _dbService.TestConnection();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task GetAllUsers_ShouldReturnList()
        {
            // Act
            var result = await _dbService.GetAllUsersAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<User>>(result);
        }

        [Fact]
        public async Task GetUserById_WithValidId_ShouldReturnUser()
        {
            // Arrange
            var users = await _dbService.GetAllUsersAsync();
            var firstUserId = users.First().Id;

            // Act
            var result = await _dbService.GetUserByIdAsync(firstUserId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<User>(result);
            Assert.Equal(firstUserId, result.Id);
        }

        [Fact]
        public async Task PostUser_ShouldCreateAndReturnUser()
        {
            // Arrange
            var newUser = new User
            {
                Id = "NewUser",
                Username = $"testuser_{Guid.NewGuid()}",
                Email = $"test_{Guid.NewGuid()}@test.com",
                PasswordHash = "test123",
                FirstName = "Test",
                LastName = "User"
            };

            // Act
            var result = await _dbService.PostUserAsync(newUser);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(newUser.Username, result.Username);
            Assert.Equal(newUser.Email, result.Email);

            // Cleanup
            await _dbService.DeleteUserAsync("NewUser");
        }
    }
} 