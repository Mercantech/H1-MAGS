using Npgsql;

namespace Domain_Models
{
    public class Item : Common
    {
        public double Price { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;

        public User Seller { get; set; } = new User();
        public string SellerId { get; set; } = string.Empty;

        public Item MapFromSQL(NpgsqlDataReader reader)
        {
            return new Item
            {
                Id = reader.GetString(reader.GetOrdinal("id")),
                Name = reader.GetString(reader.GetOrdinal("Name")),
                Description = reader.GetString(reader.GetOrdinal("Description")),
                Price = (double)reader.GetDecimal(reader.GetOrdinal("Price")),
                ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl")),
                Brand = reader.GetString(reader.GetOrdinal("Brand")),
                Seller = new User
                {
                    Username = reader.GetString(reader.GetOrdinal("SellerUsername")),
                    Email = reader.GetString(reader.GetOrdinal("SellerEmail")),
                    FirstName = reader.GetString(reader.GetOrdinal("SellerFirstName")),
                    LastName = reader.GetString(reader.GetOrdinal("SellerLastName"))
                }
            };
        }
    }
}