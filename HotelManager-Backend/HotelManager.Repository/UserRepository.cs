using HotelManager.Model;
using HotelManager.Model.Common;
using HotelManager.Repository.Common;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        public async Task<IUser> GetByIdAsync(Guid id)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
            {
                IUser profile = null;
                string commandText = "SELECT * FROM \"User\" WHERE \"Id\" = @Id AND \"IsActive\" = TRUE";
                using (NpgsqlCommand npgsqlCommand = new NpgsqlCommand(commandText, connection))
                {
                    npgsqlCommand.Parameters.AddWithValue("@Id", id);
                    try
                    {
                        await connection.OpenAsync();

                        using (NpgsqlDataReader reader = await npgsqlCommand.ExecuteReaderAsync())
                        {
                            if(await reader.ReadAsync())
                            {
                                profile = new User()
                                {
                                    Id = (Guid)reader["Id"],
                                    FirstName = (String)reader["FirstName"],
                                    LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                    Email = reader.GetString(reader.GetOrdinal("Email")),
                                    Password = reader.GetString(reader.GetOrdinal("Password")),
                                    Phone = reader.IsDBNull(reader.GetOrdinal("Phone")) ? null : reader.GetString(reader.GetOrdinal("Phone")),
                                    RoleId = reader.GetGuid(reader.GetOrdinal("RoleId")),
                                    CreatedBy = reader.GetGuid(reader.GetOrdinal("CreatedBy")),
                                    UpdatedBy = reader.GetGuid(reader.GetOrdinal("UpdatedBy")),
                                    CreatedDate = reader.GetDateTime(reader.GetOrdinal("DateCreated")),
                                    UpdatedDate = reader.GetDateTime(reader.GetOrdinal("DateUpdated")),
                                    IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
                                };
                            }
                        }
                    }catch (Exception ex) 
                    {
                        throw ex;
                    }
                }
                return profile;
            }
        }

        public async Task<bool> CreateAsync(IUser newProfile)
        {
            int rowChanged;
            byte[] salt = GenerateSalt();
            string hashedPassword = HashPassword(newProfile.Password, salt);
            
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            using (connection)
            {
                string insertQuery = "INSERT INTO \"User\" (\"Id\", \"FirstName\", \"LastName\", \"Email\", \"Password\", \"Salt\", \"Phone\", \"RoleId\", \"CreatedBy\", \"UpdatedBy\", \"IsActive\") " +
                    "VALUES (@Id, @FirstName, @LastName, @Email, @Password, @Salt, @Phone, (SELECT \"Id\" FROM \"Role\" WHERE \"Name\" = 'User'), @CreatedBy, @UpdatedBy, @IsActive)";

                NpgsqlCommand insertCommand = new NpgsqlCommand(insertQuery, connection);
                //required parameters
                insertCommand.Parameters.Add("@Id", NpgsqlDbType.Uuid).Value = newProfile.Id;
                insertCommand.Parameters.Add("@FirstName", NpgsqlDbType.Char).Value = newProfile.FirstName;
                insertCommand.Parameters.Add("@LastName", NpgsqlDbType.Char).Value = newProfile.LastName;
                insertCommand.Parameters.Add("@Email", NpgsqlDbType.Text).Value = newProfile.Email;
                insertCommand.Parameters.Add("@Password", NpgsqlDbType.Text).Value = hashedPassword;
                insertCommand.Parameters.Add("@Salt", NpgsqlDbType.Text).Value = Convert.ToBase64String(salt);
                insertCommand.Parameters.Add("@CreatedBy", NpgsqlDbType.Uuid).Value = newProfile.CreatedBy;
                insertCommand.Parameters.Add("@UpdatedBy", NpgsqlDbType.Uuid).Value = newProfile.UpdatedBy;
                insertCommand.Parameters.Add("@IsActive", NpgsqlDbType.Boolean).Value = newProfile.IsActive;

                //not required parameters
                NpgsqlParameter phoneParam = new NpgsqlParameter("@Phone", NpgsqlDbType.Text);
                phoneParam.Value = (object)newProfile.Phone ?? DBNull.Value;
                insertCommand.Parameters.Add(phoneParam);

                try
                {
                    connection.Open();
                    rowChanged = await insertCommand.ExecuteNonQueryAsync();
                    return rowChanged != 0;
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

        public async Task<bool> UpdateAsync(Guid id, IUser updatedProfile)
        {
            int rowsChanged;

            IUser profile = await GetByIdAsync(id);
            if (profile == null)
            {
                throw new Exception("No user with such ID in the database!");
            }

            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                string updateQuery = "UPDATE \"User\" SET";
                List<string> updateFields = new List<string>();

                if (!string.IsNullOrEmpty(updatedProfile.FirstName))
                {
                    updateFields.Add("\"FirstName\" = @FirstName");
                }
                if (!string.IsNullOrEmpty(updatedProfile.LastName))
                {
                    updateFields.Add("\"LastName\" = @LastName");
                }
                if (!string.IsNullOrEmpty(updatedProfile.Email))
                {
                    updateFields.Add("\"Email\" = @Email");
                }
                if (!string.IsNullOrEmpty(updatedProfile.Phone))
                {
                    updateFields.Add("\"Phone\" = @Phone");
                }

                updateQuery += " " + string.Join(", ", updateFields);
                updateQuery += " WHERE \"Id\" = @Id AND \"IsActive\" = TRUE";

                NpgsqlCommand updateCommand = new NpgsqlCommand(updateQuery, connection);
                AddProfileParameters(updateCommand, id, updatedProfile);

                rowsChanged = await updateCommand.ExecuteNonQueryAsync();
            }
            return rowsChanged != 0;
        }



        public async Task<bool> UpdatePasswordAsync(Guid id, string passwordNew, string passwordOld)
        {
            int rowsChanged;
            byte[] salt = GenerateSalt();
            string hashedPassword = HashPassword(passwordNew, salt);
            IUser currUser = await ValidateUserByPasswordAsync(id, passwordOld);
            string oldPassword = currUser.Password;
            if(hashedPassword == oldPassword)
            {
                return false;
            }
            IUser profile = await GetByIdAsync(id) ?? throw new Exception("No user with such ID in the database!");
            
            if(passwordOld != hashedPassword) { 
            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                string updateCommand = $"UPDATE \"User\" SET \"Password\" = @password, \"Salt\"=@salt WHERE \"Id\"=@id AND \"IsActive\" = TRUE";
                NpgsqlCommand command = new NpgsqlCommand(updateCommand, connection);
                command.Parameters.AddWithValue("password", hashedPassword);
                command.Parameters.AddWithValue("salt", Convert.ToBase64String(salt));
                command.Parameters.AddWithValue("id", id);
                rowsChanged = await command.ExecuteNonQueryAsync();
            }
            return rowsChanged != 0;
            }
            return false;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
            {
                string deleteQuery = "UPDATE \"User\" SET \"IsActive\" = false WHERE \"Id\" = @Id";
                NpgsqlCommand deleteCommand = new NpgsqlCommand(deleteQuery, connection);
                deleteCommand.Parameters.AddWithValue("@Id", id);

                try
                {
                    await connection.OpenAsync();

                    int rowsAffected = await deleteCommand.ExecuteNonQueryAsync();

                    return rowsAffected > 0;
                }
                catch (NpgsqlException ex)
                {
                    throw ex;
                }
            }
        }


        public async Task<IUser> ValidateUserByPasswordAsync(Guid id, string password)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
            {
                IUser user = null;
                string commandText = "SELECT \"User\".\"Id\", \"User\".\"Email\", \"User\".\"Password\", \"User\".\"RoleId\", \"User\".\"Salt\" FROM \"User\" WHERE \"User\".\"Id\" = @id AND \"IsActive\" = TRUE";
                using (NpgsqlCommand npgsqlCommand = new NpgsqlCommand(commandText, connection))
                {
                    npgsqlCommand.Parameters.AddWithValue("id", id);
                    try
                    {
                        await connection.OpenAsync();
                        using (NpgsqlDataReader reader = await npgsqlCommand.ExecuteReaderAsync())
                        {
                            if (reader.Read())
                            {
                                user = new User()
                                {
                                    Id = (Guid)reader["Id"],
                                    RoleId = (Guid)reader["RoleId"]
                                };
                                string storedPassword = (String)reader["Password"];
                                byte[] salt = Convert.FromBase64String((String)reader["Salt"]);

                                string hashedPassword = HashPassword(password, salt);

                                if (hashedPassword != storedPassword)
                                {
                                    user = null;
                                }
                                
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                return user;
            }
        }

        public async Task<IUser> ValidateUserAsync(string email, string password)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
            {
                IUser user = null;
                string commandText = "SELECT \"User\".\"Id\", \"User\".\"Email\", \"User\".\"Password\", \"User\".\"RoleId\", \"User\".\"Salt\"  FROM \"User\" WHERE \"Email\" = @Email AND \"IsActive\" = TRUE";
                using (NpgsqlCommand npgsqlCommand = new NpgsqlCommand(commandText, connection))
                {
                    npgsqlCommand.Parameters.AddWithValue("@Email", email);
                    try
                    {
                        await connection.OpenAsync();
                        using (NpgsqlDataReader reader = await npgsqlCommand.ExecuteReaderAsync())
                        {
                            if (reader.Read())
                            {
                                user = new User()
                                {
                                    Id = (Guid)reader["Id"],
                                    RoleId = (Guid)reader["RoleId"],
                                };
                                string storedPassword = (String)reader["Password"];
                                byte[] salt = Convert.FromBase64String((String)reader["Salt"]);

                                string hashedPassword = HashPassword(password, salt);

                                if (hashedPassword != storedPassword)
                                {
                                    user = null;
                                }
                                
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                return user;
            }
        }

        private void AddProfileParameters(NpgsqlCommand command, Guid id, IUser updatedProfile)
        {
            command.Parameters.AddWithValue("@Id", id);
            if (!string.IsNullOrEmpty(updatedProfile.FirstName))
            {
                command.Parameters.AddWithValue("@FirstName", updatedProfile.FirstName);
            }
            if (!string.IsNullOrEmpty(updatedProfile.LastName))
            {
                command.Parameters.AddWithValue("@LastName", updatedProfile.LastName);
            }
            if (!string.IsNullOrEmpty(updatedProfile.Email))
            {
                command.Parameters.AddWithValue("@Email", updatedProfile.Email);
            }
            if (!string.IsNullOrEmpty(updatedProfile.Password))
            {
                command.Parameters.AddWithValue("@Password", updatedProfile.Password);
            }
            if (!string.IsNullOrEmpty(updatedProfile.Phone))
            {
                command.Parameters.AddWithValue("@Phone", updatedProfile.Phone);
            }
            if (updatedProfile.RoleId != null)
            {
                command.Parameters.AddWithValue("@RoleId", updatedProfile.RoleId);
            }
            if (updatedProfile.CreatedBy != null)
            {
                command.Parameters.AddWithValue("@CreatedBy", updatedProfile.CreatedBy);
            }
            if (updatedProfile.UpdatedBy != null)
            {
                command.Parameters.AddWithValue("@UpdatedBy", updatedProfile.UpdatedBy);
            }
            if (updatedProfile.CreatedDate != null)
            {
                command.Parameters.AddWithValue("@DateCreated", updatedProfile.CreatedDate);
            }
            if (updatedProfile.UpdatedDate != null)
            {
                command.Parameters.AddWithValue("@DateUpdated", updatedProfile.UpdatedDate);
            }
            if (updatedProfile.IsActive != null)
            {
                command.Parameters.AddWithValue("@IsActive", updatedProfile.IsActive);
            }
        }

        public static byte[] GenerateSalt()
        {
            byte[] salt = new byte[32]; 
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        public static string HashPassword(string password, byte[] salt)
        {
            using (var sha256 = new SHA256Managed())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] saltedPassword = new byte[passwordBytes.Length + salt.Length];

                Buffer.BlockCopy(passwordBytes, 0, saltedPassword, 0, passwordBytes.Length);
                Buffer.BlockCopy(salt, 0, saltedPassword, passwordBytes.Length, salt.Length);

                byte[] hashedBytes = sha256.ComputeHash(saltedPassword);

                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }
    }
}
