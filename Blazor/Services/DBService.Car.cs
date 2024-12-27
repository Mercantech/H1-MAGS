using Domain_Models;
using Npgsql;

namespace Blazor.Services
{
    public partial class DBService
    {
        public async Task<List<PetrolCar>> GetAllPetrolCarsAsync()
        {
            var cars = new List<PetrolCar>();
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = GetPetrolCars;
                
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var car = new PetrolCar().MapFromSQL(reader);
                        cars.Add(car);
                    }
                }
            }
            return cars;
        }
        public async Task<List<EVCar>> GetAllEVCarsAsync()
        {
            var cars = new List<EVCar>();
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = GetEVCars;
                
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var car = new EVCar
                        {
                            Id = reader.GetString(reader.GetOrdinal("Id")),
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


        public static string GetPetrolCars => @"
            SELECT 
                v.id,
                v.Name,
                v.Description,
                v.Price,
                v.ImageUrl,
                v.Brand,
                v.Model,
                v.Year,
                v.Color,
                v.Mileage,
                p.EngineSize,
                p.HorsePower,
                p.Torque,
                p.FuelEfficiency,
                p.FuelType,
                u.username as SellerUsername,
                u.email as SellerEmail,
                u.first_name as SellerFirstName,
                u.last_name as SellerLastName
            FROM Vehicles v
            INNER JOIN PetrolDetails p ON v.id = p.VehicleId
            INNER JOIN ""User"" u ON v.SellerId = u.id
            WHERE v.VehicleType = 'Petrol'";

        public static string GetEVCars => @"
            SELECT 
                v.id,
                v.Name,
                v.Description,
                v.Price,
                v.ImageUrl,
                v.Brand,
                v.Model,
                v.Year,
                v.Color,
                v.Mileage,
                ev.BatteryCapacity,
                ev.Range,
                ev.ChargeTime,
                ev.FastCharge,
                u.username as SellerUsername,
                u.email as SellerEmail,
                u.first_name as SellerFirstName,
                u.last_name as SellerLastName
            FROM Vehicles v
            INNER JOIN EVDetails ev ON v.id = ev.VehicleId
            INNER JOIN ""User"" u ON v.SellerId = u.id
            WHERE v.VehicleType = 'EV'";
        }

    
}
