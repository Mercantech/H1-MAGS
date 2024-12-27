using Npgsql;

namespace Domain_Models
{
    public class PetrolCar : Car
    {
        public double EngineSize { get; set; } = 0;
        public double HorsePower { get; set; } = 0;
        public double Torque { get; set; } = 0;
        public double FuelEfficiency { get; set; } = 0;
        public string FuelType { get; set; } = ""; // Diesel, Gasoline, etc.

        public new PetrolCar MapFromSQL(NpgsqlDataReader reader)
        {
            var baseCar = base.MapFromSQL(reader);
            return new PetrolCar
            {
                // Arv alle properties fra base Car
                Id = baseCar.Id,
                Name = baseCar.Name,
                Description = baseCar.Description,
                Price = baseCar.Price,
                ImageUrl = baseCar.ImageUrl,
                Brand = baseCar.Brand,
                Seller = baseCar.Seller,
                Model = baseCar.Model,
                Year = baseCar.Year,
                Color = baseCar.Color,
                Mileage = baseCar.Mileage,
                
                // Tilføj Petrol-specifikke properties
                EngineSize = (double)reader.GetDecimal(reader.GetOrdinal("EngineSize")),
                HorsePower = reader.GetInt32(reader.GetOrdinal("HorsePower")),
                Torque = (double)reader.GetDecimal(reader.GetOrdinal("Torque")),
                FuelEfficiency = (double)reader.GetDecimal(reader.GetOrdinal("FuelEfficiency")),
                FuelType = reader.GetString(reader.GetOrdinal("FuelType"))
            };
        }
    }

    
}
