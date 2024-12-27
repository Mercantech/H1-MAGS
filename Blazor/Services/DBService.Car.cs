using Domain_Models;
using Npgsql;

namespace Blazor.Services
{
    public partial class DBService
    {
        public async Task<List<PetrolCar>> GetAllPetrolCarsAsync()
        {
            var sql = await ReadSqlFileAsync("SQL-Scripts/Get/PetrolCars.sql");
            Console.WriteLine("SQL: " + sql);
            var cars = new List<PetrolCar>();
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = sql;
            
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var car = new PetrolCar().MapFromSQL(reader);
                        cars.Add(car);
                    }
                }
            }
            Console.WriteLine("Cars: " + cars.Count);
            return cars;
        }
        public async Task<List<EVCar>> GetAllEVCarsAsync()
        {
            var sql = await ReadSqlFileAsync("SQL-Scripts/Get/EVCars.sql");
            var cars = new List<EVCar>();
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = sql;
                
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var car = new EVCar
                        {
                            Id = reader.GetString(reader.GetOrdinal("VehicleId")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Description = reader.GetString(reader.GetOrdinal("Description")),
                            Price = (double)reader.GetDecimal(reader.GetOrdinal("Price")),
                            ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl")),
                            Brand = reader.GetString(reader.GetOrdinal("Brand")),
                            Model = reader.GetString(reader.GetOrdinal("Model")),
                            Year = reader.GetInt32(reader.GetOrdinal("Year")),
                            Color = reader.GetString(reader.GetOrdinal("Color")),
                            Mileage = reader.GetInt32(reader.GetOrdinal("Mileage")),
                            BatteryCapacity = reader.GetDouble(reader.GetOrdinal("BatteryCapacity")),
                            Range = reader.GetDouble(reader.GetOrdinal("Range")),
                            ChargeTime = reader.GetDouble(reader.GetOrdinal("ChargeTime")),
                            FastCharge = reader.GetDouble(reader.GetOrdinal("FastCharge")),
                            Seller = new User
                            {
                                Username = reader.GetString(reader.GetOrdinal("SellerUsername")),
                                Email = reader.GetString(reader.GetOrdinal("SellerEmail")),
                                FirstName = reader.GetString(reader.GetOrdinal("SellerFirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("SellerLastName"))
                            }
                        };
                        cars.Add(car);
                    }
                }
            }
            
            return cars;
        }
        public async Task<List<Car>> GetAllCarsAsync()
        {
            List<Car> cars = new List<Car>();
            cars.AddRange(await GetAllPetrolCarsAsync());
            cars.AddRange(await GetAllEVCarsAsync());
            return cars;
        }
    }

    
}
