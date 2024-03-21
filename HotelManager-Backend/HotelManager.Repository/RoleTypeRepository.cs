using HotelManager.Repository.Common;
using Npgsql;
using System;
using System.Configuration;
using System.Threading.Tasks;

namespace HotelManager.Repository
{
    public class RoleTypeRepository : IRoleTypeRepository
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        public async Task<string> GetByIdAsync(Guid id)
        {
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            using (connection)
            {
                NpgsqlCommand command = new NpgsqlCommand();
                command.CommandText = "SELECT * FROM \"Role\" WHERE \"Id\" = @Id AND \"IsActive\" = TRUE";
                command.Connection = connection;
                command.Parameters.AddWithValue("@Id", id);
                await connection.OpenAsync();
                NpgsqlDataReader reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    return (string)reader["Name"];
                }
                return "RoleType not found";
            }   
        }
    }
}
