using HotelManager.Common;
using HotelManager.Model.Common;
using HotelManager.Model;
using HotelManager.Repository.Common;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.Repository
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

        public async Task<PagedList<IDiscount>> GetAllDiscountsAsync(DiscountFilter filter, Sorting sorting, Paging paging)
        {
            var itemCount = 0;
            List<IDiscount> discounts = new List<IDiscount>();
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            using (connection)
            {
                NpgsqlCommand command = new NpgsqlCommand();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM \"Discount\" WHERE \"Discount\".\"IsActive\"=true";
                ApplyFilter(command, filter);
                ApplySorting(command, sorting);
                itemCount = await GetItemCountAsync(filter);
                ApplyPaging(command, paging, itemCount);

                try
                {
                    await connection.OpenAsync();
                    NpgsqlDataReader reader = await command.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        discounts.Add(new Discount()
                        {
                            Id = (Guid)reader["Id"],
                            Code = (string)reader["Code"],
                            Percent = (int)reader["Percent"],
                            ValidFrom = (DateTime)reader["ValidFrom"],
                            ValidTo = (DateTime)reader["ValidTo"],
                            CreatedBy = (Guid)reader["CreatedBy"],
                            UpdatedBy = (Guid)reader["UpdatedBy"],
                            DateCreated = (DateTime)reader["DateCreated"],
                            DateUpdated = (DateTime)reader["DateUpdated"],
                            IsActive = (bool)reader["IsActive"]
                        });
                    }
                }
                catch (NpgsqlException e)
                {
                    throw e;
                }
                finally
                {
                    await connection.CloseAsync();
                }
            }
            return new PagedList<IDiscount>(discounts,paging.PageNumber,paging.PageSize,itemCount);
        }
        public async Task<IDiscount> GetDiscountByIdAsync(Guid id)
        {
            IDiscount discount = null;
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            string commandText = $"SELECT * FROM \"Discount\" WHERE \"Id\" = @id AND \"Discount\".\"IsActive\"=true ";
            using (connection)
            {
                NpgsqlCommand command = new NpgsqlCommand();
                command.CommandText = commandText;
                command.Connection = connection;
                command.Parameters.AddWithValue("id", id);
                try
                {
                    await connection.OpenAsync();
                    NpgsqlDataReader reader = await command.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        if (discount == null)
                        {
                            discount = new Discount()
                            {
                                Id = (Guid)reader.GetGuid(reader.GetOrdinal("Id")),
                                Code = (string)reader["Code"],
                                Percent = (int)reader["Percent"],
                                ValidFrom = reader.GetDateTime(reader.GetOrdinal("ValidFrom")),
                                ValidTo = reader.GetDateTime(reader.GetOrdinal("ValidTo")),
                                CreatedBy = (Guid)reader.GetGuid(reader.GetOrdinal("CreatedBy")),
                                UpdatedBy = (Guid)reader.GetGuid(reader.GetOrdinal("UpdatedBy")),
                                DateCreated = reader.GetDateTime(reader.GetOrdinal("DateCreated")),
                                DateUpdated = reader.GetDateTime(reader.GetOrdinal("DateUpdated")),
                                IsActive = (bool)reader["IsActive"]
                            };
                        }
                    }
                }
                catch (NpgsqlException e)
                {
                    throw e;
                }
                finally
                {
                    await connection.CloseAsync();
                }
            }
            return discount;
        }

        public async Task<string> UpdateDiscountAsync(Guid id, IDiscount updatedDiscount, Guid userId)
        {
            if (updatedDiscount == null)
            {
                return ("No new parameters given");
            }
            IDiscount discountToUpdate = await GetDiscountByIdAsync(id);
            if (discountToUpdate == null)
            {
                return ("Discount not found");
            }
            else
            {
                NpgsqlCommand command = new NpgsqlCommand();
                StringBuilder commandText = new StringBuilder();
                commandText.Append($"UPDATE \"Discount\" SET ");
                if (updatedDiscount.Code != discountToUpdate.Code)
                {
                    commandText.Append($"\"Code\"=@code, ");
                    command.Parameters.AddWithValue("code", updatedDiscount.Code);
                }
                if (updatedDiscount.Percent != discountToUpdate.Percent)
                {
                    commandText.Append($"\"Percent\"=@percent, ");
                    command.Parameters.AddWithValue("percent", updatedDiscount.Percent);
                }
                if (updatedDiscount.ValidFrom != discountToUpdate.ValidFrom)
                {
                    commandText.Append($"\"ValidFrom\"=@validFrom, ");
                    command.Parameters.AddWithValue("validFrom", updatedDiscount.ValidFrom);
                }
                if (updatedDiscount.ValidTo != discountToUpdate.ValidTo)
                {
                    commandText.Append($"\"ValidTo\"=@validTo, ");
                    command.Parameters.AddWithValue("validTo", updatedDiscount.ValidTo);
                }
                commandText.Append($"\"DateUpdated\"=@dateUpdated, ");
                command.Parameters.AddWithValue("dateUpdated", updatedDiscount.DateUpdated);
                commandText.Append("\"UpdatedBy\"=@userId ");
                command.Parameters.AddWithValue("userId", updatedDiscount.UpdatedBy);
                commandText.Append(" WHERE \"Id\"=@id AND \"IsActive\" = TRUE");
                command.Parameters.AddWithValue("id", id);
                command.CommandText = commandText.ToString();
                if (commandText.Length == 0)
                {
                    return ("No changes made");
                }


                NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
                command.Connection = connection;
                int numberOfAffectedRows;
                using (connection)
                {
                    connection.Open();
                    numberOfAffectedRows = await command.ExecuteNonQueryAsync();
                    connection.Close();
                }
                if (numberOfAffectedRows > 0)
                {
                    return ("Discount updated");
                }
                return ("Discount not updated");
            }
        }

        public async Task<string> CreateDiscountAsync(IDiscount newDiscount, Guid userId)
        {
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            int numberOfAffectedRows;

            using (connection)
            {
                string insert = $"INSERT INTO \"Discount\" (\"Id\",\"Code\",\"Percent\",\"ValidFrom\",\"ValidTo\",\"CreatedBy\",\"UpdatedBy\", \"DateCreated\", \"DateUpdated\") VALUES (@id,@code,@percent,@validFrom,@validTo,@createdBy,@updatedBy,@dateCreated,@dateUpdated)";
                NpgsqlCommand command = new NpgsqlCommand(insert, connection);
                connection.Open();
                command.Parameters.AddWithValue("id", newDiscount.Id);
                command.Parameters.AddWithValue("code", newDiscount.Code);
                command.Parameters.AddWithValue("percent", newDiscount.Percent);
                command.Parameters.AddWithValue("validFrom", newDiscount.ValidFrom);
                command.Parameters.AddWithValue("validTo", newDiscount.ValidTo);
                command.Parameters.AddWithValue("createdBy", newDiscount.CreatedBy);
                command.Parameters.AddWithValue("updatedBy", newDiscount.UpdatedBy);
                command.Parameters.AddWithValue("dateCreated", newDiscount.DateCreated);
                command.Parameters.AddWithValue("dateUpdated", newDiscount.DateUpdated);
                numberOfAffectedRows = await command.ExecuteNonQueryAsync();
                connection.Close();
            }

            if (numberOfAffectedRows > 0)
            {
                return ("Discount added");
            }
            return ("Discount not added");
        }


        public async Task<int> DeleteDiscountActiveAsync(Guid id)
        {
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            using (connection)
            {
                NpgsqlCommand command = new NpgsqlCommand();
                command.CommandText = $"UPDATE \"Discount\" " +
                    $"SET \"IsActive\" = false " + $"WHERE \"Id\" = @id";
                command.Connection = connection;
                command.Parameters.AddWithValue("id", id);
                try
                {
                    await connection.OpenAsync();
                    return await command.ExecuteNonQueryAsync();
                }
                catch (NpgsqlException e)
                {
                    throw e;
                }
                finally
                {
                    await connection.CloseAsync();
                }
            }
        }

        private void ApplyFilter(NpgsqlCommand command, DiscountFilter filter)
        {
            StringBuilder commandText = new StringBuilder(command.CommandText);
            if (filter.StartingValue > 0 || filter.EndValue < 100)
            {
                commandText.Append(" AND \"Discount\".\"Percent\" BETWEEN @startValue AND @endValue");
                command.Parameters.AddWithValue("startValue", filter.StartingValue);
                command.Parameters.AddWithValue("endValue", filter.EndValue);
            }
            if(filter.Code != "")
            {
                commandText.Append(" AND \"Discount\".\"Code\" LIKE @code");
                command.Parameters.AddWithValue("code", filter.Code);
            }
            command.CommandText = commandText.ToString();
        }

        private void ApplySorting(NpgsqlCommand command, Sorting sorting)
        {
            StringBuilder commandText = new StringBuilder(command.CommandText);
            commandText.Append(" ORDER BY \"");
            commandText.Append(sorting.SortBy).Append("\" ");
            commandText.Append(sorting.SortOrder == "ASC" ? "ASC" : "DESC");
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


        public async Task<int> GetItemCountAsync(DiscountFilter filter)
        {
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            using (connection)
            {
                NpgsqlCommand command = new NpgsqlCommand();
                command.CommandText = "SELECT COUNT(\"Id\") FROM \"Discount\"";
                ApplyFilter(command, filter);
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

    }
}