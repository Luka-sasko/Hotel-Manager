using HotelManager.Common;
using HotelManager.Model;
using HotelManager.Repository.Common;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.Repository
{
    public class RoomTypeRepository : IRoomTypeRepository
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;


        public async Task<int> GetItemCountAsync()
        {
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            using (connection)
            {
                NpgsqlCommand command = new NpgsqlCommand();
                command.CommandText = "SELECT COUNT(\"Id\") FROM \"RoomType\" r WHERE r.\"IsActive\" = TRUE";
                command.Connection = connection;
                try
                {
                    await connection.OpenAsync();
                    object result = await command.ExecuteScalarAsync();
                    return Convert.ToInt32(result);
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

        public async Task<PagedList<RoomType>> GetAllAsync(Paging paging, Sorting sorting)
        {
            var roomTypes = new List<RoomType>();
            var totalCount = 0;

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var queryBuilder = new StringBuilder();
                queryBuilder.AppendLine("SELECT * FROM \"RoomType\" WHERE \"IsActive\" = TRUE");

                using (var cmd = new NpgsqlCommand())
                {
                    if (sorting != null && !string.IsNullOrEmpty(sorting.SortBy))
                    {
                        queryBuilder.Append(" ORDER BY ");
                        queryBuilder.Append($"\"{sorting.SortBy}\"");

                        if (!string.IsNullOrEmpty(sorting.SortOrder))
                        {
                            queryBuilder.Append(" ");
                            queryBuilder.Append(sorting.SortOrder);
                        }
                    }
                     totalCount = await GetItemCountAsync();

                    if (paging != null)
                    {
                        cmd.Parameters.AddWithValue("@Limit", paging.PageSize);
                        cmd.Parameters.AddWithValue("@Offset", (paging.PageNumber - 1) * paging.PageSize);
                        queryBuilder.Append(" LIMIT @Limit OFFSET @Offset");
                    }
                    cmd.Connection = connection;
                    cmd.CommandText = queryBuilder.ToString();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var roomType = new RoomType
                            {
                                Id = (Guid)(reader["Id"]),
                                Name = (String)reader["Name"],
                                Description = (String)reader["Description"],
                                CreatedBy = (Guid)reader["CreatedBy"],
                                UpdatedBy = (Guid)reader["UpdatedBy"],
                                DateCreated = (DateTime)reader["DateCreated"],
                                DateUpdated = (DateTime)reader["DateUpdated"],
                                IsActive = (bool)reader["IsActive"]
                            };
                            roomTypes.Add(roomType);
                        }
                    }
                }
            }
            return new PagedList<RoomType>(roomTypes, paging.PageNumber, paging.PageSize, totalCount);
        }

        public async Task<RoomType> GetByIdAsync(Guid id)
        {
            var roomType = new RoomType();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = "SELECT * FROM \"RoomType\" WHERE \"Id\" = @Id AND \"IsActive\" = TRUE";

                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Id", id);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            roomType = new RoomType
                            {
                                Id = (Guid)reader["Id"],
                                Name = (string)reader["Name"],
                                Description = (string)reader["Description"],
                                CreatedBy = (Guid)reader["CreatedBy"],
                                UpdatedBy = (Guid)reader["UpdatedBy"],
                                DateCreated = (DateTime)reader["DateCreated"],
                                DateUpdated = (DateTime)reader["DateUpdated"],
                                IsActive = (bool)reader["IsActive"]
                            };
                        }
                    }
                }
            }
            return roomType;
        }

        public async Task<RoomType> PostAsync(RoomType roomType, Guid userId)
        {
            Guid creationId = Guid.NewGuid();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = "INSERT INTO \"RoomType\" (\"Id\", \"Name\", \"Description\", \"IsActive\", \"CreatedBy\", \"UpdatedBy\", \"DateCreated\", \"DateUpdated\") " +
                            "VALUES (@Id, @Name, @Description, @IsActive, @CreatedBy, @UpdatedBy, @DateCreated, @DateUpdated)";

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Parameters.AddWithValue("@Id",creationId);
                    cmd.Parameters.AddWithValue("@Name",roomType.Name);
                    cmd.Parameters.AddWithValue("@Description", roomType.Description);
                    cmd.Parameters.AddWithValue("@IsActive", roomType.IsActive);
                    // set user who created
                    cmd.Parameters.AddWithValue("@CreatedBy", roomType.CreatedBy);
                    cmd.Parameters.AddWithValue("@UpdatedBy", roomType.UpdatedBy);
                    cmd.Parameters.AddWithValue("@DateCreated", roomType.DateCreated);
                    cmd.Parameters.AddWithValue("@DateUpdated", roomType.DateUpdated);

                    cmd.Connection = connection;
                    cmd.CommandText = query.ToString();
                    await cmd.ExecuteNonQueryAsync();
                }
                
            }
            return await GetByIdAsync(creationId);

        }

        public async Task<RoomTypeUpdate> UpdateAsync(Guid id, RoomTypeUpdate roomTypeUpdate, Guid userId)
        {
            RoomType roomType = await GetByIdAsync(id);

            if (roomTypeUpdate == null)
                return null;
            if (roomType == null)
                return null;


            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var queryBuilder = new StringBuilder();
                queryBuilder.AppendLine("UPDATE \"RoomType\" SET ");

                using (var cmd = new NpgsqlCommand())
                {
                    if (roomTypeUpdate.Name != null)
                    {
                        cmd.Parameters.AddWithValue("@Name", roomTypeUpdate.Name);
                        queryBuilder.AppendLine(" \"Name\" = @Name,");
                    }

                    if (roomTypeUpdate.Description != null)
                    {
                        cmd.Parameters.AddWithValue("@Description", roomTypeUpdate.Description);
                        queryBuilder.AppendLine(" \"Description\" = @Description,");
                    }

                    if (roomTypeUpdate.IsActive != default)
                    {
                        cmd.Parameters.AddWithValue("@IsActive", roomTypeUpdate.IsActive);
                        queryBuilder.AppendLine(" \"IsActive\" = @IsActive,");
                    }

                    cmd.Parameters.AddWithValue("@DateUpdated", roomTypeUpdate.DateUpdated);
                    queryBuilder.AppendLine(" \"DateUpdated\" = @DateUpdated,");

                    // need update on userId who updated
                    cmd.Parameters.AddWithValue("@UpdatedBy", roomTypeUpdate.UpdatedBy);
                    queryBuilder.AppendLine(" \"UpdatedBy\" = @UpdatedBy");

                    cmd.Parameters.AddWithValue("@id", id);
                    queryBuilder.AppendLine(" WHERE \"Id\" = @id AND \"IsActive\" = TRUE");


                    cmd.Connection = connection;
                    cmd.CommandText = queryBuilder.ToString();
                    await cmd.ExecuteNonQueryAsync();


                    roomType = await GetByIdAsync(id);
                    RoomTypeUpdate editedRoomType = new RoomTypeUpdate();
                    editedRoomType = SetValue(roomType, editedRoomType);
                    return editedRoomType;
                }
            }

        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = "UPDATE \"RoomType\" SET \"IsActive\" = FALSE WHERE \"Id\" = @Id";

                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    int rowsAffected = await cmd.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }


        private RoomTypeUpdate SetValue(RoomType roomType, RoomTypeUpdate editedRoomType)
        {
            editedRoomType.UpdatedBy = roomType.UpdatedBy;
            editedRoomType.DateUpdated = roomType.DateUpdated;
            editedRoomType.Description = roomType.Description;
            editedRoomType.Name=roomType.Name;
            editedRoomType.IsActive = roomType.IsActive;
            return editedRoomType;
        }
    }
}
