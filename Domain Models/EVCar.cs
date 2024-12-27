using Npgsql;

namespace Domain_Models
{
    public class EVCar : Car
    {
        public double BatteryCapacity { get; set; } = 0;
        public double Range { get; set; } = 0;
        public double ChargeTime { get; set; } = 0;
        public double FastCharge { get; set; } = 0;

        public new EVCar MapFromSQL(NpgsqlDataReader reader)
        {
            var baseCar = base.MapFromSQL(reader);
            return new EVCar
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
                
                // Tilføj EV-specifikke properties
                BatteryCapacity = (double)reader.GetDecimal(reader.GetOrdinal("BatteryCapacity")),
                Range = (double)reader.GetDecimal(reader.GetOrdinal("Range")),
                ChargeTime = (double)reader.GetDecimal(reader.GetOrdinal("ChargeTime")),
                FastCharge = (double)reader.GetDecimal(reader.GetOrdinal("FastCharge"))
            };
        }
    }
}