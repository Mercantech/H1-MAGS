namespace Blazor.Services
{
    public partial class DBService
    {
        public List<string> GetAllTables()
        {
            List<string> tables = new List<string>();
            
            try
            {
                using var connection = new Npgsql.NpgsqlConnection(_connectionString);
                connection.Open();

                string sql = @"
                    SELECT table_name 
                    FROM information_schema.tables
                    WHERE table_schema = 'public'";

                using var cmd = new Npgsql.NpgsqlCommand(sql, connection);
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    tables.Add(reader.GetString(0));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fejl ved hentning af tabeller: {ex.Message}");
            }

            return tables;
        }
    }
} 