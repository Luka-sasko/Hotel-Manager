using HotelManager.Common;
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
    public class RoomRepository : IRoomRepository
    {

        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

        public async Task<int> GetItemCountAsync(RoomFilter filter)
        {
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            using (connection)
            {
                NpgsqlCommand command = new NpgsqlCommand();
                command.CommandText = "SELECT COUNT(DISTINCT r.\"Id\") FROM \"Room\" r " +
                              "LEFT JOIN \"Reservation\" res ON r.\"Id\" = res.\"RoomId\" " +
                              "WHERE r.\"IsActive\" = TRUE";
                ApplyFilter(command, filter);
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

        public async Task<PagedList<Room>> GetAllAsync(Paging paging, Sorting sorting, RoomFilter roomFilter)
        {
            var rooms = new List<Room>();

            var totalCount = 0;

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var queryBuilder = new StringBuilder();
                queryBuilder.AppendLine("SELECT DISTINCT r.*");
                queryBuilder.AppendLine(" FROM \"Room\" r");
                queryBuilder.AppendLine(" JOIN \"RoomType\" rt ON r.\"TypeId\" = rt.\"Id\"");
                queryBuilder.AppendLine(" LEFT JOIN \"Reservation\" res ON r.\"Id\" = res.\"RoomId\"");
                queryBuilder.AppendLine(" WHERE 1=1");
                queryBuilder.AppendLine(" AND r.\"IsActive\" = true");

                using (var cmd = new NpgsqlCommand())
                {
                    if (roomFilter != null)
                    {
                        if (roomFilter.StartDate != null && roomFilter.EndDate != null)
                        {
                            cmd.Parameters.AddWithValue("@StartDate", roomFilter.StartDate);
                            cmd.Parameters.AddWithValue("@EndDate", roomFilter.EndDate);
                            queryBuilder.AppendLine(" AND NOT EXISTS (\r\n    SELECT 1\r\n    FROM \"Reservation\" rsv\r\n    WHERE rsv.\"RoomId\" = r.\"Id\"\r\n    AND rsv.\"CheckOutDate\" >= @StartDate\r\n    AND rsv.\"CheckInDate\" <= @EndDate\r\n)");
                        }

                        if (roomFilter.MinPrice != null && roomFilter.MinPrice > 0)
                        {
                            cmd.Parameters.AddWithValue("@MinPrice", roomFilter.MinPrice);
                            queryBuilder.AppendLine(" AND r.\"Price\" >= @MinPrice::money");
                            if (roomFilter.MaxPrice != null && roomFilter.MaxPrice > roomFilter.MinPrice)
                            {
                                cmd.Parameters.AddWithValue("@MaxPrice", roomFilter.MaxPrice);
                                queryBuilder.AppendLine(" AND r.\"Price\" <= @MaxPrice::money");
                            }
                        }
                        else if (roomFilter.MaxPrice != null && roomFilter.MaxPrice > 0)
                        {
                            cmd.Parameters.AddWithValue("@MaxPrice", roomFilter.MaxPrice);
                            queryBuilder.AppendLine(" AND r.\"Price\" <= @MaxPrice::money");
                        }

                        if (roomFilter.MinBeds > 0)
                        {
                            cmd.Parameters.AddWithValue("@MinBeds", roomFilter.MinBeds);
                            queryBuilder.AppendLine(" AND r.\"BedCount\" >= @MinBeds");
                        }

                        if (roomFilter.RoomTypeId != null)
                        {
                            cmd.Parameters.AddWithValue("@RoomTypeId", roomFilter.RoomTypeId);
                            queryBuilder.AppendLine(" AND r.\"TypeId\" = @RoomTypeId");
                        }
                    }

                    if (sorting != null && !string.IsNullOrEmpty(sorting.SortBy))
                    {
                        queryBuilder.Append(" ORDER BY \"");
                        queryBuilder.Append(sorting.SortBy);
                        queryBuilder.Append("\"");

                        if (!string.IsNullOrEmpty(sorting.SortOrder))
                        {
                            queryBuilder.Append(" ");
                            queryBuilder.Append(sorting.SortOrder);
                        }
                    }

                    totalCount = await GetItemCountAsync(roomFilter);

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
                            rooms.Add(new Room
                            {
                                Id = reader.GetGuid(reader.GetOrdinal("Id")),
                                Number = reader.GetInt32(reader.GetOrdinal("Number")),
                                BedCount = reader.GetInt32(reader.GetOrdinal("BedCount")),
                                Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                                ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl")),
                                TypeId = reader.GetGuid(reader.GetOrdinal("TypeId")),
                                CreatedBy = reader.GetGuid(reader.GetOrdinal("CreatedBy")),
                                UpdatedBy = reader.GetGuid(reader.GetOrdinal("UpdatedBy")),
                                DateCreated = reader.GetDateTime(reader.GetOrdinal("DateCreated")),
                                DateUpdated = reader.GetDateTime(reader.GetOrdinal("DateUpdated")),
                                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
                            });
                        }
                    }
                }
            }
            return new PagedList<Room>(rooms, paging.PageNumber, paging.PageSize, totalCount);
        }


        public async Task<Room> GetByIdAsync(Guid id)
        {
            Room room = null;

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = "SELECT r.*, rt.\"Name\" AS TypeName " +
            "FROM \"Room\" r " +
            "JOIN \"RoomType\" rt ON r.\"TypeId\" = rt.\"Id\" " +
            "WHERE r.\"Id\" = @Id";



                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Id", id);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            room = new Room
                            {
                                Id = reader.GetGuid(reader.GetOrdinal("Id")),
                                Number = reader.GetInt32(reader.GetOrdinal("Number")),
                                BedCount = reader.GetInt32(reader.GetOrdinal("BedCount")),
                                Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                                IsAvailable = reader.GetBoolean(reader.GetOrdinal("IsAvailable")),
                                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                                ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl")),
                                TypeId = reader.GetGuid(reader.GetOrdinal("TypeId")),
                                TypeName = reader.GetString(reader.GetOrdinal("TypeName"))
                            };
                        }
                    }
                }
            }

            return room;
        }


        public async Task<RoomUpdate> UpdateRoomAsync(Guid id, RoomUpdate roomUpdate, Guid userId)
        {
            Room room = await GetByIdAsync(id);

            if (roomUpdate == null)
                return null;
            if (room == null)
                return null;


            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var queryBuilder = new StringBuilder();
                queryBuilder.AppendLine("UPDATE \"Room\" SET ");

                using (var cmd = new NpgsqlCommand())
                {
                    if (roomUpdate.Number != null)
                    {
                        cmd.Parameters.AddWithValue("@Number", roomUpdate.Number);
                        queryBuilder.AppendLine(" \"Number\" = @Number,");
                    }

                    if (roomUpdate.BedCount != null)
                    {
                        cmd.Parameters.AddWithValue("@BedCount", roomUpdate.BedCount);
                        queryBuilder.AppendLine(" \"BedCount\" = @BedCount,");
                    }

                    if (roomUpdate.Price != null)
                    {
                        cmd.Parameters.AddWithValue("@Price", roomUpdate.Price);
                        queryBuilder.AppendLine(" \"Price\" = @Price,");
                    }

                    if (roomUpdate.ImageUrl != null)
                    {
                        cmd.Parameters.AddWithValue("@ImageUrl", roomUpdate.ImageUrl);
                        queryBuilder.AppendLine(" \"ImageUrl\" = @ImageUrl,");
                    }

                    if (roomUpdate.TypeId != null)
                    {
                        cmd.Parameters.AddWithValue("@TypeId", roomUpdate.TypeId);
                        queryBuilder.AppendLine(" \"TypeId\" = @TypeId,");
                    }

                    if (roomUpdate.IsActive != null)
                    {
                        cmd.Parameters.AddWithValue("@IsActive", roomUpdate.IsActive);
                        queryBuilder.AppendLine(" \"IsActive\" = @IsActive,");
                    }


                    cmd.Parameters.AddWithValue("@DateUpdated", roomUpdate.DateUpdated);
                    queryBuilder.AppendLine(" \"DateUpdated\" = @DateUpdated,");

                    cmd.Parameters.AddWithValue("@UpdatedBy", roomUpdate.UpdatedBy);
                    queryBuilder.AppendLine(" \"UpdatedBy\" = @UpdatedBy");

                    cmd.Parameters.AddWithValue("@id", id);
                    queryBuilder.AppendLine(" WHERE \"Id\" = @id");


                    cmd.Connection = connection;
                    cmd.CommandText = queryBuilder.ToString();
                    await cmd.ExecuteNonQueryAsync();

                }
            }

            Room editedRoom = await GetByIdAsync(id);
            RoomUpdate roomUpdated = new RoomUpdate();
            SetValues(editedRoom, roomUpdated);
            return roomUpdated;
        }


        public async Task<RoomUpdate> GetRoomUpdateByIdAsync(Guid id)
        {
            Room room = await GetByIdAsync(id);
            RoomUpdate roomUpdate = new RoomUpdate();

            SetValues(room, roomUpdate);

            return roomUpdate;
        }

        public async Task<PagedList<RoomUpdate>> GetUpdatedRoomsAsync(Paging paging, Sorting sorting, RoomFilter roomsFilter)
        {
            var updatedRooms = new List<RoomUpdate>();

            var totalCount = 0;

            if (roomsFilter != null)
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var queryBuilder = new StringBuilder();
                    queryBuilder.AppendLine("SELECT DISTINCT r.*");
                    queryBuilder.AppendLine(" FROM \"Room\" r");
                    queryBuilder.AppendLine(" JOIN \"RoomType\" rt ON r.\"TypeId\" = rt.\"Id\"");
                    queryBuilder.AppendLine(" LEFT JOIN \"Reservation\" res ON r.\"Id\" = res.\"RoomId\"");
                    queryBuilder.AppendLine(" WHERE 1=1");
                    queryBuilder.AppendLine(" AND r.\"IsActive\" = true");

                    using (var cmd = new NpgsqlCommand())
                    {
                        if (roomsFilter.StartDate != null && roomsFilter.EndDate != null)
                        {
                            cmd.Parameters.AddWithValue("@StartDate", roomsFilter.StartDate);
                            cmd.Parameters.AddWithValue("@EndDate", roomsFilter.EndDate);
                            queryBuilder.AppendLine(" AND (res.\"CheckInDate\" >= @StartDate AND res.\"CheckInDate\" <= @EndDate)");
                        }

                        if (roomsFilter.MinPrice != null && roomsFilter.MaxPrice != null)
                        {
                            cmd.Parameters.AddWithValue("@MinPrice", roomsFilter.MinPrice);
                            cmd.Parameters.AddWithValue("@MaxPrice", roomsFilter.MaxPrice);
                            queryBuilder.AppendLine(" AND r.\"Price\" BETWEEN @MinPrice::money AND @MaxPrice::money");
                        }

                        if (roomsFilter.MinBeds > 0)
                        {
                            cmd.Parameters.AddWithValue("@MinBeds", roomsFilter.MinBeds);
                            queryBuilder.AppendLine(" AND r.\"BedCount\" >= @MinBeds");
                        }

                        if (roomsFilter.RoomTypeId != null)
                        {
                            cmd.Parameters.AddWithValue("@RoomTypeId", roomsFilter.RoomTypeId);
                            queryBuilder.AppendLine(" AND r.\"TypeId\" = @RoomTypeId");
                        }

                        if (sorting != null && !string.IsNullOrEmpty(sorting.SortBy))
                        {
                            queryBuilder.Append(" ORDER BY ");
                            queryBuilder.Append(sorting.SortBy);

                            if (!string.IsNullOrEmpty(sorting.SortOrder))
                            {
                                queryBuilder.Append(" ");
                                queryBuilder.Append(sorting.SortOrder);
                            }
                        }

                        totalCount = await GetItemCountAsync(roomsFilter);

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
                                var roomUpdate = new RoomUpdate();
                                SetValues(new Room
                                {
                                    Id = (Guid)reader["Id"],
                                    Number = (int)reader["Number"],
                                    BedCount = (int)reader["BedCount"],
                                    Price = (decimal)reader["Price"],
                                    ImageUrl = (string)reader["ImageUrl"],
                                    TypeId = (Guid)reader["TypeId"],
                                    CreatedBy = (Guid)reader["CreatedBy"],
                                    UpdatedBy = (Guid)reader["UpdatedBy"],
                                    DateCreated = (DateTime)reader["DateCreated"],
                                    DateUpdated = (DateTime)reader["DateUpdated"],
                                    IsActive = (bool)reader["IsActive"]
                                }, roomUpdate);

                                updatedRooms.Add(roomUpdate);
                            }
                        }
                    }
                }
            }
            return new PagedList<RoomUpdate>(updatedRooms, paging.PageNumber, paging.PageSize, totalCount);
        }


        private void ApplyFilter(NpgsqlCommand command, RoomFilter roomFilter)
        {
            if (roomFilter != null)
            {
                if (roomFilter.StartDate != null && roomFilter.EndDate != null)
                {
                    command.Parameters.AddWithValue("@StartDate", roomFilter.StartDate);
                    command.Parameters.AddWithValue("@EndDate", roomFilter.EndDate);
                    command.CommandText += " AND NOT EXISTS (" +
                                          "SELECT 1 FROM \"Reservation\" rsv " +
                                          "WHERE rsv.\"RoomId\" = r.\"Id\" " +
                                          "AND rsv.\"CheckOutDate\" >= @StartDate " +
                                          "AND rsv.\"CheckInDate\" <= @EndDate)";
                }

                if (roomFilter.MinPrice != null && roomFilter.MaxPrice != null)
                {
                    command.Parameters.AddWithValue("@MinPrice", roomFilter.MinPrice);
                    command.Parameters.AddWithValue("@MaxPrice", roomFilter.MaxPrice);
                    command.CommandText += " AND r.\"Price\" BETWEEN @MinPrice::money AND @MaxPrice::money";
                }

                if (roomFilter.MinBeds > 0)
                {
                    command.Parameters.AddWithValue("@MinBeds", roomFilter.MinBeds);
                    command.CommandText += " AND r.\"BedCount\" >= @MinBeds";
                }

                if (roomFilter.RoomTypeId != null)
                {
                    command.Parameters.AddWithValue("@RoomTypeId", roomFilter.RoomTypeId);
                    command.CommandText += " AND r.\"TypeId\" = @RoomTypeId";
                }
            }
        }

        private static void SetValues(Room room, RoomUpdate roomUpdate)
        {
            roomUpdate.Id = room.Id;
            roomUpdate.BedCount = room.BedCount;
            roomUpdate.Number = room.Number;
            roomUpdate.DateUpdated = room.DateUpdated;
            roomUpdate.UpdatedBy = room.UpdatedBy;
            roomUpdate.Price = room.Price;
            roomUpdate.ImageUrl = room.ImageUrl;
            roomUpdate.IsActive = room.IsActive;
            roomUpdate.TypeId = room.TypeId;
        }

        public async Task<Room> PostRoomAsync(Room room)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = @"
            INSERT INTO ""Room"" (
                ""Id"", ""Number"", ""BedCount"", ""Price"", ""IsActive"", ""ImageUrl"", ""TypeId"", ""CreatedBy"", ""UpdatedBy"", ""DateCreated"", ""DateUpdated""
            ) VALUES (
                @Id, @Number, @BedCount, @Price, @IsActive, @ImageUrl, @TypeId, @CreatedBy, @UpdatedBy, @DateCreated, @DateUpdated
            )
            RETURNING *
        ";

                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Id", room.Id);
                    cmd.Parameters.AddWithValue("@Number", room.Number);
                    cmd.Parameters.AddWithValue("@BedCount", room.BedCount);
                    cmd.Parameters.AddWithValue("@Price", room.Price);
                    cmd.Parameters.AddWithValue("@IsActive", room.IsActive);
                    cmd.Parameters.AddWithValue("@ImageUrl", room.ImageUrl);
                    cmd.Parameters.AddWithValue("@TypeId", room.TypeId);
                    cmd.Parameters.AddWithValue("@CreatedBy", room.CreatedBy);
                    cmd.Parameters.AddWithValue("@UpdatedBy", room.UpdatedBy);
                    cmd.Parameters.AddWithValue("@DateCreated", room.DateCreated);
                    cmd.Parameters.AddWithValue("@DateUpdated", room.DateUpdated);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new Room
                            {
                                Id = reader.GetGuid(reader.GetOrdinal("Id")),
                                Number = reader.GetInt32(reader.GetOrdinal("Number")),
                                BedCount = reader.GetInt32(reader.GetOrdinal("BedCount")),
                                Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                                ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl")),
                                TypeId = reader.GetGuid(reader.GetOrdinal("TypeId")),
                                CreatedBy = reader.GetGuid(reader.GetOrdinal("CreatedBy")),
                                UpdatedBy = reader.GetGuid(reader.GetOrdinal("UpdatedBy")),
                                DateCreated = reader.GetDateTime(reader.GetOrdinal("DateCreated")),
                                DateUpdated = reader.GetDateTime(reader.GetOrdinal("DateUpdated"))
                            };
                        }
                    }
                }
            }

            return null; // Return null if insertion fails
        }

    }
}
