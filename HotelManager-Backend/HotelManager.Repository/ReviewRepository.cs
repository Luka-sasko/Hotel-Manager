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
using System.Xml.Linq;

namespace HotelManager.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

        
        public async Task<int> GetItemCountAsync(ReviewFilter filter)
        {
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            using (connection)
            {
                NpgsqlCommand command = new NpgsqlCommand();
                command.CommandText = "SELECT COUNT(\"Id\") FROM \"Review\" r WHERE r.\"IsActive\" = TRUE";
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

        public async Task<PagedList<Review>> GetAllAsync(Paging paging, Sorting sorting, ReviewFilter reviewFilter)
        {
            List<Review> reviews = new List<Review>();
            var totalCount = 0;

            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                StringBuilder queryBuilder = new StringBuilder();
                queryBuilder.Append("SELECT r.\"Id\", r.\"Rating\", r.\"Comment\", r.\"UserId\", r.\"RoomId\", ");
                queryBuilder.Append("r.\"CreatedBy\", r.\"UpdatedBy\", r.\"DateCreated\", r.\"DateUpdated\", ");
                queryBuilder.Append("r.\"IsActive\", u.\"FirstName\", u.\"LastName\" ");
                queryBuilder.Append("FROM \"Review\" r ");
                queryBuilder.Append("JOIN \"User\" u ON r.\"UserId\" = u.\"Id\" ");
                queryBuilder.Append("WHERE r.\"IsActive\" = TRUE ");

                NpgsqlCommand command = new NpgsqlCommand();
                command.Connection = connection;
                command.CommandText = queryBuilder.ToString();
                ApplyFilter(command, reviewFilter);
                queryBuilder.Clear();
                totalCount = await GetItemCountAsync(reviewFilter);

                queryBuilder.Append(" LIMIT @Limit OFFSET @Offset");

                command.CommandText += queryBuilder.ToString();

                command.Parameters.AddWithValue("@Limit", paging.PageSize);
                command.Parameters.AddWithValue("@Offset", (paging.PageNumber - 1) * paging.PageSize);

                using (NpgsqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        Review review = new Review
                        {
                            Id = Guid.Parse(reader["Id"].ToString()),
                            Rating = Convert.ToInt32(reader["Rating"]),
                            Comment = reader["Comment"].ToString(),
                            UserId = Guid.Parse(reader["UserId"].ToString()),
                            UserFullName = reader["FirstName"].ToString() + " " + reader["LastName"].ToString(),
                            RoomId = Guid.Parse(reader["RoomId"].ToString()),
                            CreatedBy = Guid.Parse(reader["CreatedBy"].ToString()),
                            UpdatedBy = Guid.Parse(reader["UpdatedBy"].ToString()),
                            DateCreated = ((DateTime)reader["DateCreated"]).Date,
                            DateUpdated = (DateTime)reader["DateUpdated"],
                            IsActive = (bool)reader["IsActive"]
                        };
                        reviews.Add(review);
                    }
                }
            }
            return new PagedList<Review>(reviews, paging.PageNumber, paging.PageSize, totalCount);
        }


        private void ApplyFilter(NpgsqlCommand command, ReviewFilter reviewFilter)
        {
            if (reviewFilter != null)
            {
                if (reviewFilter.Rating > 0)
                {
                    command.CommandText += " AND r.\"Rating\" >= @MinRating ";
                    command.Parameters.AddWithValue("@MinRating", reviewFilter.Rating);
                }
                if (reviewFilter.RoomId != Guid.Empty)
                {
                    command.CommandText += " AND r.\"RoomId\" = @RoomId ";
                    command.Parameters.AddWithValue("@RoomId", reviewFilter.RoomId);
                }
            }
        }

        public async Task<bool> CreateAsync(Review review)
        {
            int rowsChanged = 0;

            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "INSERT INTO \"Review\" (\"Id\", \"Rating\", \"Comment\", \"UserId\", \"RoomId\", \"CreatedBy\", \"UpdatedBy\", \"DateCreated\", \"DateUpdated\", \"IsActive\") " +
               "VALUES (@Id, @Rating, @Comment, @UserId, @RoomId, @CreatedBy, @UpdatedBy, @DateCreated, @DateUpdated, @IsActive)";

                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {

                    command.Parameters.AddWithValue("@Id", review.Id);
                    command.Parameters.AddWithValue("@Rating", review.Rating);
                    command.Parameters.AddWithValue("@Comment", review.Comment);
                    command.Parameters.AddWithValue("@UserId", review.CreatedBy);
                    command.Parameters.AddWithValue("@RoomId", review.RoomId);
                    command.Parameters.AddWithValue("@CreatedBy", review.CreatedBy);
                    command.Parameters.AddWithValue("@UpdatedBy", review.UpdatedBy);
                    command.Parameters.AddWithValue("@DateCreated", review.DateCreated);
                    command.Parameters.AddWithValue("@DateUpdated", review.DateUpdated);
                    command.Parameters.AddWithValue("@IsActive", review.IsActive);

                    rowsChanged = await command.ExecuteNonQueryAsync();
                }
            }

            return rowsChanged > 0;
        }
    }
}

