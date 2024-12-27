using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Blazor.Services
{
    public partial class DBService
    {

        

        public async Task InsertDummyData()
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            // Indsæt kategorier
            var categories = new[]
            {
                "Sedan", "SUV", "Sports", "Luxury", "Compact"
            };
            
            //foreach (var category in categories)
            //{
            //    await using var cmd = connection.CreateCommand();
            //    cmd.CommandText = @"
            //        INSERT INTO Categories (id, Name) 
            //        SELECT @id, @name 
            //        WHERE NOT EXISTS (
            //            SELECT 1 FROM Categories WHERE Name = @name
            //        )";
            //    cmd.Parameters.AddWithValue("@id", Guid.NewGuid().ToString());
            //    cmd.Parameters.AddWithValue("@name", category);
            //    await cmd.ExecuteNonQueryAsync();
            //}

            // Indsæt dummy brugere
            var users = new[]
            {
                ("john_doe", "john@example.com", "John", "Doe"),
                ("jane_smith", "jane@example.com", "Jane", "Smith"),
                ("anders_hansen", "anders@example.com", "Anders", "Hansen"),
                ("simon_jensen", "simon@example.com", "Simon", "Jensen"),
                ("kristian_larsen", "kristian@example.com", "Kristian", "Larsen"),
                ("mathias_thomsen", "mathias@example.com", "Mathias", "Thomsen"),
                ("jonas_pedersen", "jonas@example.com", "Jonas", "Pedersen"),
                ("michael_jensen", "michael@example.com", "Michael", "Jensen"),
                ("lars_andersen", "lars@example.com", "Lars", "Andersen"),
                ("christian_nielsen", "christian@example.com", "Christian", "Nielsen"),
                ("jonas_jensen", "jonas@example.com", "Jonas", "Jensen"),
                ("heidi_jensen", "heidi@example.com", "Heidi", "Jensen"),
            };

            //foreach (var (username, email, firstName, lastName) in users)
            //{
            //    await using var cmd = connection.CreateCommand();
            //    cmd.CommandText = @"
            //        INSERT INTO ""User"" (id, username, email, password_hash, first_name, last_name) 
            //        VALUES (@id, @username, @email, @password_hash, @firstName, @lastName)
            //        ON CONFLICT (username) DO NOTHING";
                
            //    cmd.Parameters.AddWithValue("@id", Guid.NewGuid().ToString());
            //    cmd.Parameters.AddWithValue("@username", username);
            //    cmd.Parameters.AddWithValue("@email", email);
            //    cmd.Parameters.AddWithValue("@password_hash", "password123");
            //    cmd.Parameters.AddWithValue("@firstName", firstName);
            //    cmd.Parameters.AddWithValue("@lastName", lastName);
            //    await cmd.ExecuteNonQueryAsync();
            //}

            // Indsæt dummy biler
            var evCars = new[]
            {
                new { Name = "Tesla Model 3", Brand = "Tesla", Price = 450000.00, Category = "Sedan", 
                      BatteryCapacity = 75.0, Range = 450.0, ChargeTime = 8.0, FastCharge = 170.0 },
                new { Name = "VW ID.4", Brand = "Volkswagen", Price = 380000.00, Category = "SUV", 
                      BatteryCapacity = 82.0, Range = 520.0, ChargeTime = 7.5, FastCharge = 135.0 },
                new { Name = "Tesla Model S", Brand = "Tesla", Price = 550000.00, Category = "Sports", 
                      BatteryCapacity = 100.0, Range = 620.0, ChargeTime = 10.0, FastCharge = 180.0 },
                new { Name = "Tesla Model X", Brand = "Tesla", Price = 650000.00, Category = "SUV", 
                      BatteryCapacity = 100.0, Range = 620.0, ChargeTime = 10.0, FastCharge = 180.0 },
                new { Name = "Tesla Model Y", Brand = "Tesla", Price = 550000.00, Category = "SUV", 
                      BatteryCapacity = 100.0, Range = 620.0, ChargeTime = 10.0, FastCharge = 180.0 }, 
                new { Name = "Skoda Enyaq", Brand = "Skoda", Price = 350000.00, Category = "SUV", 
                      BatteryCapacity = 82.0, Range = 520.0, ChargeTime = 7.5, FastCharge = 135.0 },
                new { Name = "Volvo C40", Brand = "Volvo", Price = 450000.00, Category = "Sedan", 
                      BatteryCapacity = 75.0, Range = 450.0, ChargeTime = 8.0, FastCharge = 170.0 }
            };

            var petrolCars = new[]
            {
                new { Name = "BMW 320i", Brand = "BMW", Price = 520000.00, Category = "Sedan",
                      EngineSize = 2.0, HorsePower = 184, Torque = 300.0, FuelEfficiency = 15.5 },
                new { Name = "Audi A4", Brand = "Audi", Price = 495000.00, Category = "Sedan",
                      EngineSize = 2.0, HorsePower = 190, Torque = 320.0, FuelEfficiency = 14.8 },
                new { Name = "Volvo S60", Brand = "Volvo", Price = 550000.00, Category = "Sedan",
                      EngineSize = 2.0, HorsePower = 190, Torque = 320.0, FuelEfficiency = 14.8 },
                new { Name = "Volvo V60", Brand = "Volvo", Price = 550000.00, Category = "Sedan",
                      EngineSize = 2.0, HorsePower = 190, Torque = 320.0, FuelEfficiency = 14.8 },
                new { Name = "Volvo V90", Brand = "Volvo", Price = 550000.00, Category = "Sedan",
                      EngineSize = 2.0, HorsePower = 190, Torque = 320.0, FuelEfficiency = 14.8 },
                new { Name = "Volvo XC40", Brand = "Volvo", Price = 550000.00, Category = "SUV",    
                      EngineSize = 2.0, HorsePower = 190, Torque = 320.0, FuelEfficiency = 14.8 }
            };

            // Indsæt el-biler
            foreach (var car in evCars)
            {
                await using var cmd = connection.CreateCommand();
                cmd.CommandText = @"
                    WITH category_id AS (
                        SELECT c.id FROM Categories c WHERE c.Name = @category LIMIT 1
                    ),
                    random_seller AS (
                        SELECT u.id FROM ""User"" u ORDER BY RANDOM() LIMIT 1
                    )
                    INSERT INTO Vehicles (id, Name, Brand, Price, CategoryId, VehicleType, SellerId)
                    SELECT @id, @name, @brand, @price, 
                           (SELECT id FROM category_id),
                           'EV',
                           (SELECT id FROM random_seller)
                    WHERE EXISTS (SELECT 1 FROM category_id) 
                    AND EXISTS (SELECT 1 FROM random_seller)
                    RETURNING id";

                cmd.Parameters.AddWithValue("@id", Guid.NewGuid().ToString());
                cmd.Parameters.AddWithValue("@name", car.Name);
                cmd.Parameters.AddWithValue("@brand", car.Brand);
                cmd.Parameters.AddWithValue("@price", car.Price);
                cmd.Parameters.AddWithValue("@category", car.Category);
                cmd.Parameters.AddWithValue("@batteryCapacity", car.BatteryCapacity);
                cmd.Parameters.AddWithValue("@range", car.Range);
                cmd.Parameters.AddWithValue("@chargeTime", car.ChargeTime);
                cmd.Parameters.AddWithValue("@fastCharge", car.FastCharge);

                var vehicleId = await cmd.ExecuteScalarAsync() as string;
                // ... resten af EV details koden ...
            }

            // Indsæt benzinbiler
            foreach (var car in petrolCars)
            {
                await using var cmd = connection.CreateCommand();
                cmd.CommandText = @"
                    WITH category_id AS (
                        SELECT c.id FROM Categories c WHERE c.Name = @category LIMIT 1
                    ),
                    random_seller AS (
                        SELECT u.id FROM ""User"" u ORDER BY RANDOM() LIMIT 1
                    )
                    INSERT INTO Vehicles (id, Name, Brand, Model, Price, CategoryId, VehicleType, SellerId)
                    SELECT @id, @name, @brand, @brand, @price, 
                           (SELECT id FROM category_id),
                           'Petrol',
                           (SELECT id FROM random_seller)
                    WHERE EXISTS (SELECT 1 FROM category_id) 
                    AND EXISTS (SELECT 1 FROM random_seller)
                    RETURNING id";

                cmd.Parameters.AddWithValue("@id", Guid.NewGuid().ToString());
                cmd.Parameters.AddWithValue("@name", car.Name);
                cmd.Parameters.AddWithValue("@brand", car.Brand);
                cmd.Parameters.AddWithValue("@price", car.Price);
                cmd.Parameters.AddWithValue("@category", car.Category);

                var vehicleId = await cmd.ExecuteScalarAsync() as string;

                if (vehicleId != null)
                {
                    await using var detailsCmd = connection.CreateCommand();
                    detailsCmd.CommandText = @"
                        INSERT INTO PetrolDetails 
                        (VehicleId, EngineSize, HorsePower, Torque, FuelEfficiency, FuelType)
                        VALUES 
                        (@vehicleId, @engineSize, @horsePower, @torque, @fuelEfficiency, 'Petrol')";

                    detailsCmd.Parameters.AddWithValue("@vehicleId", vehicleId);
                    detailsCmd.Parameters.AddWithValue("@engineSize", car.EngineSize);
                    detailsCmd.Parameters.AddWithValue("@horsePower", car.HorsePower);
                    detailsCmd.Parameters.AddWithValue("@torque", car.Torque);
                    detailsCmd.Parameters.AddWithValue("@fuelEfficiency", car.FuelEfficiency);

                    await detailsCmd.ExecuteNonQueryAsync();
                }
            }
        }
    }
}