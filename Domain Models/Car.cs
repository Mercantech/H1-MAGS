using Npgsql;

namespace Domain_Models
{
    public class Car : Item
    {
        public string Model { get; set; } = string.Empty;
        public int? Year { get; set; } = 0;
        public string? Color { get; set; } = string.Empty;
        public int? Mileage { get; set; } = 0;

        public new Car MapFromSQL(NpgsqlDataReader reader)
        {
            var baseCar = base.MapFromSQL(reader);
            return new Car
            {
                // Arv alle properties fra base Item
                Id = baseCar.Id,
                Name = baseCar.Name,
                Description = baseCar.Description,
                Price = baseCar.Price,
                ImageUrl = baseCar.ImageUrl,
                Brand = baseCar.Brand,
                Seller = baseCar.Seller,
                
                // Tilføj Car-specifikke properties
                Model = reader.GetString(reader.GetOrdinal("Model")),
                Year = reader.GetInt32(reader.GetOrdinal("Year")),
                Color = reader.GetString(reader.GetOrdinal("Color")),
                Mileage = reader.GetInt32(reader.GetOrdinal("Mileage"))
            };
        }
    }
}