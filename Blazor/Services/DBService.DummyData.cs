using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Blazor.Services
{
    public partial class DBService
    {


        public async Task AddEVDetailsToExistingCars()
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            // Først henter vi alle EV biler uden detaljer
            var cmd = connection.CreateCommand();
            cmd.CommandText = @"
            SELECT id, Name, Brand 
            FROM Vehicles 
            WHERE VehicleType = 'EV' 
            AND id NOT IN (SELECT VehicleId FROM EVDetails)";

            var carsWithoutDetails = new List<(string Id, string Name, string Brand)>();

            await using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    carsWithoutDetails.Add((
                        reader.GetString(0),  // id
                        reader.GetString(1),  // name
                        reader.GetString(2)   // brand
                    ));
                }
            }

            // Justerede værdier for at undgå overflow
            var defaultSpecs = new Dictionary<string, (double BatteryCapacity, double Range, double ChargeTime, double FastCharge)>
            {
                ["Tesla"] = (75.0, 350.0, 8.0, 150.0),
                ["Volkswagen"] = (82.0, 320.0, 7.5, 125.0),
                ["Skoda"] = (82.0, 320.0, 7.5, 125.0),
                ["Volvo"] = (75.0, 350.0, 8.0, 150.0)
            };

            // Default værdier også justeret
            foreach (var car in carsWithoutDetails)
            {
                var specs = defaultSpecs.GetValueOrDefault(car.Brand, (70.0, 300.0, 7.0, 120.0));

                await using var detailsCmd = connection.CreateCommand();
                detailsCmd.CommandText = @"
                    INSERT INTO EVDetails 
                    (id, VehicleId, BatteryCapacity, Range, ChargeTime, FastCharge)
                    VALUES 
                    (@id, @vehicleId, @batteryCapacity, @range, @chargeTime, @fastCharge)";

                detailsCmd.Parameters.AddWithValue("@id", Guid.NewGuid().ToString());
                detailsCmd.Parameters.AddWithValue("@vehicleId", car.Id);
                detailsCmd.Parameters.AddWithValue("@batteryCapacity", specs.Item1);
                detailsCmd.Parameters.AddWithValue("@range", specs.Item2);
                detailsCmd.Parameters.AddWithValue("@chargeTime", specs.Item3);
                detailsCmd.Parameters.AddWithValue("@fastCharge", specs.Item4);

                await detailsCmd.ExecuteNonQueryAsync();
            }
        }
        public async Task AddPetrolDetailsToExistingCars()
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        // Først henter vi alle Petrol biler uden detaljer
        var cmd = connection.CreateCommand();
        cmd.CommandText = @"
            SELECT id, Name, Brand 
            FROM Vehicles 
            WHERE VehicleType = 'Petrol' 
            AND id NOT IN (SELECT VehicleId FROM PetrolDetails)";

        var carsWithoutDetails = new List<(string Id, string Name, string Brand)>();
        
        await using (var reader = await cmd.ExecuteReaderAsync())
        {
            while (await reader.ReadAsync())
            {
                carsWithoutDetails.Add((
                    reader.GetString(0),  // id
                    reader.GetString(1),  // name
                    reader.GetString(2)   // brand
                ));
            }
        }

        // Standard værdier baseret på mærke
        var defaultSpecs = new Dictionary<string, (double EngineSize, int HorsePower, double Torque, double FuelEfficiency)>
        {
            ["BMW"] = (2.0, 184, 300.0, 15.5),
            ["Audi"] = (2.0, 190, 320.0, 14.8),
            ["Volvo"] = (2.0, 190, 320.0, 14.8),
            ["Mercedes"] = (2.0, 195, 330.0, 15.0)
        };

        // Tilføj detaljer til hver bil
        foreach (var car in carsWithoutDetails)
        {
            var specs = defaultSpecs.GetValueOrDefault(car.Brand, (2.0, 180, 300.0, 15.0));
            
            await using var detailsCmd = connection.CreateCommand();
            detailsCmd.CommandText = @"
                INSERT INTO PetrolDetails 
                (id, VehicleId, EngineSize, HorsePower, Torque, FuelEfficiency, FuelType)
                VALUES 
                (@id, @vehicleId, @engineSize, @horsePower, @torque, @fuelEfficiency, 'Petrol')";

            detailsCmd.Parameters.AddWithValue("@id", Guid.NewGuid().ToString());
            detailsCmd.Parameters.AddWithValue("@vehicleId", car.Id);
            detailsCmd.Parameters.AddWithValue("@engineSize", specs.Item1);    // EngineSize
            detailsCmd.Parameters.AddWithValue("@horsePower", specs.Item2);    // HorsePower
            detailsCmd.Parameters.AddWithValue("@torque", specs.Item3);        // Torque
            detailsCmd.Parameters.AddWithValue("@fuelEfficiency", specs.Item4); // FuelEfficiency

            await detailsCmd.ExecuteNonQueryAsync();
        }
    }

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