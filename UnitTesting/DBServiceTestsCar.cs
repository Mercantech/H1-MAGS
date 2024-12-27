using Blazor.Services;
using Domain_Models;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace UnitTesting
{
    public class DBServiceTestsCar
    {
        private readonly DBService _dbService;
        private readonly string _connectionString;

        public DBServiceTestsCar()
        {
            // Opsæt configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            // Hent connection string fra appsettings
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new Exception("Connection string not found");
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
        public async Task GetAllPetrolCars_ShouldReturnList()
        {
            // Act
            var result = await _dbService.GetAllPetrolCarsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<PetrolCar>>(result);
        }

        [Fact]
        public async Task GetAllEVCars_ShouldReturnList()
        {
            // Act
            var result = await _dbService.GetAllEVCarsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<EVCar>>(result);
        }

        [Fact]

        public async Task GetAllCars_ShouldReturnList()
        {
            // Act
            var result = await _dbService.GetAllCarsAsync();
            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Car>>(result);
        }
    }
} 