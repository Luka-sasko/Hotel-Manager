using HotelManager.Common;
using HotelManager.Model;
using HotelManager.Repository.Common;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.Repository
{
    public class HotelServiceRepository : IHotelServiceRepository
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

        public async Task<PagedList<HotelService>> GetAllAsync(Paging paging, Sorting sorting, HotelServiceFilter hotelServiceFilter)
        {
            var services = new List<HotelService>();
            var count = 0;

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();


                using (var command = new NpgsqlCommand())
                {
                    command.CommandText = "SELECT \"Id\", \"Name\", \"Description\", \"Price\", \"CreatedBy\", \"UpdatedBy\", \"DateCreated\", \"DateUpdated\", \"IsActive\" FROM \"Service\" WHERE \"IsActive\" = TRUE";
                    command.Connection = connection;
                    if (hotelServiceFilter != null)
                    {
                        ApplyFilter(command, hotelServiceFilter);
                    }

                    count = await GetItemCountAsync(hotelServiceFilter);

                    var query = new StringBuilder(command.CommandText);

                    // Sorting
                    if (!string.IsNullOrWhiteSpace(sorting.SortBy))
                    {
                        query.Append($" ORDER BY \"{sorting.SortBy}\" {(sorting.SortOrder.ToUpper() == "ASC" ? "ASC" : "DESC")}");
                    }
                    else
                    {
                        query.Append(" ORDER BY \"DateCreated\" DESC");
                    }
                    command.CommandText = query.ToString();

                    ApplyPaging(command, paging, count);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var service = new HotelService
                            {
                                Id = Guid.Parse(reader["Id"].ToString()),
                                Name = reader["Name"].ToString(),
                                Description = reader["Description"].ToString(),
                                Price = (decimal)reader["Price"],
                                CreatedBy = Guid.Parse(reader["CreatedBy"].ToString()),
                                UpdatedBy = Guid.Parse(reader["UpdatedBy"].ToString()),
                                DateCreated = (DateTime)reader["DateCreated"],
                                DateUpdated = (DateTime)reader["DateUpdated"],
                                IsActive = (bool)reader["IsActive"]
                            };

                            services.Add(service);
                        }
                    }
                }
            }
           
            return new PagedList<HotelService>(services,paging.PageNumber,paging.PageSize, count);
        }

        public async Task<HotelService> GetByIdAsync(Guid id)
        {
            HotelService service = null;

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = "SELECT \"Id\", \"Name\", \"Description\", \"Price\", \"CreatedBy\", \"UpdatedBy\", \"DateCreated\", \"DateUpdated\", \"IsActive\" FROM \"Service\" WHERE \"Id\" = @Id AND \"IsActive\" = TRUE";

                using (var command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            service = new HotelService
                            {
                                Id = Guid.Parse(reader["Id"].ToString()),
                                Name = reader["Name"].ToString(),
                                Description = reader["Description"].ToString(),
                                Price = (decimal)reader["Price"],
                                CreatedBy = Guid.Parse(reader["CreatedBy"].ToString()),
                                UpdatedBy = Guid.Parse(reader["UpdatedBy"].ToString()),
                                DateCreated = (DateTime)reader["DateCreated"],
                                DateUpdated = (DateTime)reader["DateUpdated"],
                                IsActive = (bool)reader["IsActive"]
                            };
                        }
                    }
                }
            }

            return service;
        }

        public async Task<bool> CreateServiceAsync(HotelService newService)
        {
            int rowsChanged;
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);

            using (connection)
            {
                string insertQuery = "INSERT INTO \"Service\" (\"Id\", \"Name\", \"Description\", \"Price\", \"CreatedBy\", \"UpdatedBy\", \"DateCreated\", \"DateUpdated\", \"IsActive\") " +
                    "VALUES (@Id, @Name, @Description, @Price, @CreatedBy, @UpdatedBy, @DateCreated, @DateUpdated, @IsActive)";

                NpgsqlCommand insertCommand = new NpgsqlCommand(insertQuery, connection);

                newService.Id = Guid.NewGuid();

                insertCommand.Parameters.AddWithValue("@Id", newService.Id);
                insertCommand.Parameters.AddWithValue("@Name", newService.Name);
                insertCommand.Parameters.AddWithValue("@Description", newService.Description);
                insertCommand.Parameters.AddWithValue("@Price", newService.Price);
                insertCommand.Parameters.AddWithValue("@CreatedBy", newService.CreatedBy);
                insertCommand.Parameters.AddWithValue("@UpdatedBy", newService.UpdatedBy);
                insertCommand.Parameters.AddWithValue("@DateCreated", newService.DateCreated);
                insertCommand.Parameters.AddWithValue("@DateUpdated", newService.DateUpdated);
                insertCommand.Parameters.AddWithValue("@IsActive", newService.IsActive);

                try
                {
                    await connection.OpenAsync();
                    rowsChanged = await insertCommand.ExecuteNonQueryAsync();
                    return rowsChanged != 0;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public async Task<bool> UpdateServiceAsync(Guid id, HotelService service)
        {
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);

            try
            {
                string updateQuery = "UPDATE \"Service\" SET ";
                bool hasSet = false;

                if (!string.IsNullOrEmpty(service.Name))
                {
                    updateQuery += "\"Name\" = @Name";
                    hasSet = true;
                }

                if (!string.IsNullOrEmpty(service.Description))
                {
                    if (hasSet)
                        updateQuery += ", ";

                    updateQuery += "\"Description\" = @Description";
                    hasSet = true;
                }

                if (service.Price != default(decimal))
                {
                    if (hasSet)
                        updateQuery += ", ";

                    updateQuery += "\"Price\" = @Price";
                    hasSet = true;
                }

                if (!hasSet)
                {
                    return false;
                }
                updateQuery += ", \"UpdatedBy\" = @UpdatedBy, \"DateUpdated\" = @DateUpdated";
                updateQuery += " WHERE \"Id\" = @Id AND \"IsActive\" = TRUE";

                using (connection)
                {
                    NpgsqlCommand updateCommand = new NpgsqlCommand(updateQuery, connection);

                    updateCommand.Parameters.AddWithValue("@Id", id);
                    updateCommand.Parameters.AddWithValue("@Name", service.Name ?? (object)DBNull.Value);
                    updateCommand.Parameters.AddWithValue("@Description", service.Description ?? (object)DBNull.Value);
                    updateCommand.Parameters.AddWithValue("@Price", service.Price != default(decimal) ? service.Price : (object)DBNull.Value);
                    updateCommand.Parameters.AddWithValue("@UpdatedBy", service.UpdatedBy);
                    updateCommand.Parameters.AddWithValue("@DateUpdated", service.DateCreated);

                    await connection.OpenAsync();
                    int rowsAffected = await updateCommand.ExecuteNonQueryAsync();

                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }

        public async Task<bool> DeleteServiceAsync(Guid id, Guid userId)
        {
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);

            try
            {
                string updateQuery = "UPDATE \"Service\" SET \"IsActive\" = FALSE, \"DateUpdated\" = @DateUpdated, \"UpdatedBy\" = @UpdatedBy WHERE \"Id\" = @Id";

                using (connection)
                {
                    NpgsqlCommand updateCommand = new NpgsqlCommand(updateQuery, connection);

                    updateCommand.Parameters.AddWithValue("@Id", id);
                    updateCommand.Parameters.AddWithValue("@DateUpdated", DateTime.UtcNow);
                    updateCommand.Parameters.AddWithValue("@UpdatedBy", userId);

                    await connection.OpenAsync();
                    int rowsAffected = await updateCommand.ExecuteNonQueryAsync();

                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }

        public async Task<int> GetItemCountAsync(HotelServiceFilter hotelServiceFilter)
        {
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            using (connection)
            {
                NpgsqlCommand command = new NpgsqlCommand();
                command.CommandText = "SELECT COUNT(\"Id\") FROM \"Service\"";

                ApplyFilter(command, hotelServiceFilter);
                command.Connection = connection;
                try
                {
                    await connection.OpenAsync();
                    NpgsqlDataReader reader = await command.ExecuteReaderAsync();
                    await reader.ReadAsync();
                    return reader.GetInt32(0);
                }
                catch (Exception e)
                {
                    return 0;
                }
                finally
                {
                    await connection.CloseAsync();
                }
            }
        }
        private void ApplyFilter(NpgsqlCommand command, HotelServiceFilter hotelServiceFilter)
        {
            StringBuilder commandText = new StringBuilder(command.CommandText);

            if (hotelServiceFilter != null)
            {
                if (!string.IsNullOrWhiteSpace(hotelServiceFilter.SearchQuery))
                {
                    commandText.Append(" AND (\"Name\" ILIKE @SearchQuery OR \"Description\" ILIKE @SearchQuery)");
                    command.Parameters.AddWithValue("@SearchQuery", $"%{hotelServiceFilter.SearchQuery}%");
                }

                if (hotelServiceFilter.MinPrice.HasValue)
                {
                    commandText.Append(" AND \"Price\" >= @MinPrice::money");
                    command.Parameters.AddWithValue("@MinPrice", hotelServiceFilter.MinPrice);
                }

                if (hotelServiceFilter.MaxPrice.HasValue)
                {
                    commandText.Append(" AND \"Price\" <= @MaxPrice::money");
                    command.Parameters.AddWithValue("@MaxPrice", hotelServiceFilter.MaxPrice);
                }
            }

            command.CommandText = commandText.ToString();
        }
        private void ApplyPaging(NpgsqlCommand command, Paging paging, int itemCount)
        {
            StringBuilder commandText = new StringBuilder(command.CommandText);
            int currentItem = (paging.PageNumber - 1) * paging.PageSize;
            if (currentItem >= 0 && currentItem < itemCount)
            {
                commandText.Append(" LIMIT ").Append(paging.PageSize).Append(" OFFSET ").Append(currentItem);
                command.CommandText = commandText.ToString();
            }
            else
            {
                commandText.Append(" LIMIT 10");
                command.CommandText = commandText.ToString();
            }
        }


    }
}
