using Npgsql;

namespace Domain_Models
{
    public class Category : Common
    {
        public string Name { get; set; } = string.Empty;
    }
}