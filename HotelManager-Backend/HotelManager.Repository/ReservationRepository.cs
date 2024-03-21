using HotelManager.Common;
using HotelManager.Model;
using HotelManager.Model.Common;
using HotelManager.Repository.Common;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.Repository
{

    public class ReservationRepository : IReservationRepository
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

        public async Task<bool> CheckIfAvailableAsync(ReservationRoom reservationRoom)
        {
            bool isAvailable = false;
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var queryBuilder = new StringBuilder();
                queryBuilder.AppendLine("SELECT COUNT(*)");
                queryBuilder.AppendLine("FROM \"Reservation\"");
                queryBuilder.AppendLine("WHERE \"RoomId\" = @RoomId");
                queryBuilder.AppendLine("AND \"CheckInDate\" < @CheckOutDate");
                queryBuilder.AppendLine("AND \"CheckOutDate\" > @CheckInDate");
                queryBuilder.AppendLine("AND \"IsActive\" = TRUE");
                using (var cmd = new NpgsqlCommand(queryBuilder.ToString(), connection))
                {
                    cmd.Parameters.AddWithValue("@RoomId", reservationRoom.RoomId);
                    cmd.Parameters.AddWithValue("@CheckInDate", reservationRoom.CheckInDate);
                    cmd.Parameters.AddWithValue("@CheckOutDate", reservationRoom.CheckOutDate);
                    int result = Convert.ToInt32(await cmd.ExecuteScalarAsync());
                    isAvailable = result == 0;
                    
                }
                return isAvailable;
            }
        }


        public async Task<PagedList<ReservationWithUserEmail>> GetAllAsync(Paging paging, Sorting sorting, ReservationFilter reservationFilter)
        {
            var reservations = new List<ReservationWithUserEmail>();
            var itemCount = 0;
            if (reservationFilter != null)
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var queryBuilder = new StringBuilder();
                    queryBuilder.AppendLine("SELECT res.*, u.\"Email\", ro.\"Number\" AS \"RoomNumber\"");
                    queryBuilder.AppendLine(" FROM \"Reservation\" res");
                    queryBuilder.AppendLine(" JOIN \"User\" u ON res.\"UserId\" = u.\"Id\"");
                    queryBuilder.AppendLine(" JOIN \"Room\" ro ON res.\"RoomId\" = ro.\"Id\"");
                    queryBuilder.AppendLine(" WHERE res.\"IsActive\" = TRUE");

                    
                    using (var cmd = new NpgsqlCommand())
                    {

                        
                        cmd.Connection = connection;
                        cmd.CommandText = queryBuilder.ToString();

                        ApplyFilter(cmd, reservationFilter);
                        ApplySorting(cmd, sorting);
                        
                        itemCount = await GetItemCountAsync(reservationFilter);
                        ApplyPaging(cmd, paging, itemCount);
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                reservations.Add(new ReservationWithUserEmail
                                {
                                    Id = (Guid)reader["Id"],
                                    UserId = (Guid)reader["UserId"],
                                    RoomId = (Guid)reader["RoomId"],
                                    ReservationNumber = (string)reader["ReservationNumber"],
                                    PricePerNight = (decimal)reader["PricePerNight"],
                                    CheckInDate = (DateTime)reader["CheckInDate"],
                                    CheckOutDate = (DateTime)reader["CheckOutDate"],
                                    CreatedBy = (Guid)reader["CreatedBy"],
                                    UpdatedBy = (Guid)reader["UpdatedBy"],
                                    DateCreated = (DateTime)reader["DateCreated"],
                                    DateUpdated = (DateTime)reader["DateUpdated"],
                                    IsActive = (bool)reader["IsActive"],
                                    UserEmail = (string)reader["Email"],
                                    RoomNumber = (int)reader["RoomNumber"]
                                });
                            }
                        }
                    }
                }
            }
            return new PagedList<ReservationWithUserEmail>(reservations, paging.PageNumber, paging.PageSize, itemCount);
        }


        public async Task<IReservation> GetByIdAsync(Guid id)
        {

            IReservation reservation = null;
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = "SELECT * FROM \"Reservation\" WHERE \"Id\" = @Id AND \"IsActive\" = TRUE";

                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Id", id);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                           reservation = new Reservation
                            {
                               Id = (Guid)reader["Id"],
                               UserId = (Guid)reader["UserId"],
                               RoomId = (Guid)reader["RoomId"],
                               ReservationNumber = (string)reader["ReservationNumber"],
                               PricePerNight = (decimal)reader["PricePerNight"],
                               CheckInDate = (DateTime)reader["CheckInDate"],
                               CheckOutDate = (DateTime)reader["CheckOutDate"],
                               CreatedBy = (Guid)reader["CreatedBy"],
                               UpdatedBy = (Guid)reader["UpdatedBy"],
                               DateCreated = (DateTime)reader["DateCreated"],
                               DateUpdated = (DateTime)reader["DateUpdated"],
                               IsActive = (bool)reader["IsActive"],

                           };
                        }
                    }
                }
            }
            return reservation;
        }

        public async Task<Reservation> PostAsync(Reservation reservationCreate)
        {


            var dateNow = DateTime.UtcNow;

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var queryBuilder = new StringBuilder();

                queryBuilder.AppendLine("INSERT INTO \"Reservation\" (");
                queryBuilder.AppendLine("    \"Id\", \"UserId\", \"RoomId\", \"ReservationNumber\", ");
                queryBuilder.AppendLine("    \"PricePerNight\", \"CheckInDate\", \"CheckOutDate\", ");
                queryBuilder.AppendLine("    \"CreatedBy\", \"UpdatedBy\", \"DateCreated\", \"DateUpdated\", \"IsActive\"");
                queryBuilder.AppendLine(")");
                queryBuilder.AppendLine("VALUES (");
                queryBuilder.AppendLine("    @Id, @UserId, @RoomId, @ReservationNumber, ");
                queryBuilder.AppendLine("    @PricePerNight, @CheckInDate, @CheckOutDate, ");
                queryBuilder.AppendLine("    @CreatedBy, @UpdatedBy, @DateCreated, @DateUpdated, @IsActive");
                queryBuilder.AppendLine(")");

                using (var cmd = new NpgsqlCommand(queryBuilder.ToString(), connection))
                {
                    cmd.Parameters.AddWithValue("@Id", reservationCreate.Id);
                    cmd.Parameters.AddWithValue("@UserId", reservationCreate.CreatedBy);
                    cmd.Parameters.AddWithValue("@RoomId", reservationCreate.RoomId);
                    cmd.Parameters.AddWithValue("@ReservationNumber", reservationCreate.ReservationNumber);
                    cmd.Parameters.AddWithValue("@PricePerNight", reservationCreate.PricePerNight);
                    cmd.Parameters.AddWithValue("@CheckInDate", reservationCreate.CheckInDate);
                    cmd.Parameters.AddWithValue("@CheckOutDate", reservationCreate.CheckOutDate);
                    cmd.Parameters.AddWithValue("@CreatedBy", reservationCreate.CreatedBy);
                    cmd.Parameters.AddWithValue("@UpdatedBy", reservationCreate.UpdatedBy);
                    cmd.Parameters.AddWithValue("@DateCreated", dateNow);
                    cmd.Parameters.AddWithValue("@DateUpdated", dateNow);
                    cmd.Parameters.AddWithValue("@IsActive", true);

                    cmd.ExecuteNonQuery();
                }
            }
            reservationCreate.DateCreated = dateNow;
            reservationCreate.DateUpdated = dateNow;
            return reservationCreate;
        }


        public async Task<ReservationUpdate> UpdateAsync(Guid id, ReservationUpdate reservationUpdate, Guid userId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var queryBuilder = new StringBuilder();
                queryBuilder.AppendLine("UPDATE \"Reservation\"");
                queryBuilder.AppendLine("SET");

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;

                    if (reservationUpdate.CheckInDate != null)
                    {
                        queryBuilder.AppendLine("    \"CheckInDate\" = @CheckInDate,");
                        cmd.Parameters.AddWithValue("@CheckInDate", reservationUpdate.CheckInDate);
                    }

                    if (reservationUpdate.CheckOutDate != null)
                    {
                        queryBuilder.AppendLine("    \"CheckOutDate\" = @CheckOutDate,");
                        cmd.Parameters.AddWithValue("@CheckOutDate", reservationUpdate.CheckOutDate);
                    }

                    if (!string.IsNullOrEmpty(reservationUpdate.RoomNumber))
                    {
                        queryBuilder.AppendLine("    \"RoomNumber\" = @RoomNumber,");
                        cmd.Parameters.AddWithValue("@RoomNumber", reservationUpdate.RoomNumber);
                    }

                    if (reservationUpdate.PricePerNight != null)
                    {
                        queryBuilder.AppendLine("    \"PricePerNight\" = @PricePerNight,");
                        cmd.Parameters.AddWithValue("@PricePerNight", reservationUpdate.PricePerNight);
                    }

                    if (reservationUpdate.IsActive != null)
                    {
                        queryBuilder.AppendLine("    \"IsActive\" = @IsActive,");
                        cmd.Parameters.AddWithValue("@IsActive", reservationUpdate.IsActive);
                    }

                    queryBuilder.AppendLine("    \"UpdatedBy\" = @UpdatedBy,");
                    queryBuilder.AppendLine("    \"DateUpdated\" = @DateUpdated");
                    queryBuilder.AppendLine("WHERE \"Id\" = @Id AND \"IsActive\" = TRUE");

                    cmd.Parameters.AddWithValue("@UpdatedBy", userId);
                    cmd.Parameters.AddWithValue("@DateUpdated", DateTime.Now);
                    cmd.Parameters.AddWithValue("@Id", id);

                    cmd.CommandText = queryBuilder.ToString();

                    await cmd.ExecuteNonQueryAsync();
                }
            }

            return reservationUpdate;
        }
        
        private void ApplyFilter(NpgsqlCommand command, ReservationFilter reservationFilter)
        {
            StringBuilder queryBuilder = new StringBuilder(command.CommandText);

            if (reservationFilter.CheckInDate != default && reservationFilter.CheckOutDate != default)
            {
                command.Parameters.AddWithValue("@StartDate", reservationFilter.CheckInDate);
                command.Parameters.AddWithValue("@EndDate", reservationFilter.CheckOutDate);
                queryBuilder.AppendLine(" AND NOT (res.\"CheckOutDate\" >= @StartDate AND res.\"CheckInDate\" <= @EndDate)");
            }

            if (reservationFilter.MinPricePerNight != null && reservationFilter.MinPricePerNight > 0)
            {
                command.Parameters.AddWithValue("@MinPrice", reservationFilter.MinPricePerNight);
                queryBuilder.AppendLine(" AND res.\"PricePerNight\" >= @MinPrice::money");
                if (reservationFilter.MaxPricePerNight != null && reservationFilter.MaxPricePerNight > reservationFilter.MinPricePerNight)
                {
                    command.Parameters.AddWithValue("@MaxPrice", reservationFilter.MaxPricePerNight);
                    queryBuilder.AppendLine(" AND res.\"PricePerNight\" <= @MaxPrice::money");
                }
            }
            else if (reservationFilter.MaxPricePerNight != null)
            {
                command.Parameters.AddWithValue("@MaxPrice", reservationFilter.MaxPricePerNight);
                queryBuilder.AppendLine(" AND res.\"PricePerNight\" <= @MaxPrice::money");
            }

            if (!string.IsNullOrEmpty(reservationFilter.SearchQuery))
            {
                command.Parameters.AddWithValue("@SearchQuery", $"%{reservationFilter.SearchQuery}%");
                queryBuilder.AppendLine(" AND u.\"Email\" ILIKE @SearchQuery");
            }
            if(reservationFilter.UserId != null && reservationFilter.UserId != Guid.Empty)
            {
                command.Parameters.AddWithValue("@UserId", reservationFilter.UserId);
                queryBuilder.AppendLine(" AND res.\"UserId\" = @UserId");
            }

            command.CommandText = queryBuilder.ToString();
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

        private async Task<int> GetItemCountAsync(ReservationFilter filter)
        {
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            using (connection)
            {
                NpgsqlCommand command = new NpgsqlCommand();
                command.CommandText = "SELECT COUNT(\"Id\") FROM \"Reservation\" res WHERE res.\"IsActive\" = TRUE";
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

        public async Task<ReservationUpdate> DeleteAsync(Guid id)
        {
            ReservationUpdate reservationUpdate = new ReservationUpdate
            {
                IsActive = false
            };

            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var sql = "UPDATE \"Reservation\" SET \"IsActive\" = @IsActive WHERE \"Id\" = @Id";

                    using (var cmd = new NpgsqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@IsActive", reservationUpdate.IsActive);
                        cmd.Parameters.AddWithValue("@Id", id);

                        await cmd.ExecuteNonQueryAsync();
                    }
                }

                return reservationUpdate;
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating reservation: " + ex.Message);
            }
        }


    }
}
