using HotelManager.Common;
using HotelManager.Model;
using HotelManager.Model.Common;
using HotelManager.Repository.Common;
using HotelManager.WebApi.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.Repository
{
    public class ServiceInvoiceRepository : IServiceInvoiceRepository
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

        public async Task<PagedList<IServiceInvoice>> GetAllInvoiceServiceAsync(Sorting sorting, Paging paging)
        {
            var itemCount = 0;
            List<IServiceInvoice> invoiceService = new List<IServiceInvoice>();
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            using (connection)
            {
                NpgsqlCommand command = new NpgsqlCommand();
                command.Connection = connection;
                command.CommandText = $"SELECT * FROM \"InvoiceService\" WHERE \"InvoiceService\".\"IsActive\"=true";
                if(sorting != null)
                    ApplySorting(command, sorting);
                itemCount = await GetItemCountAsync();
                if(paging != null)
                    ApplyPaging(command, paging, itemCount);

                try
                {
                    await connection.OpenAsync();
                    NpgsqlDataReader reader = await command.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        invoiceService.Add(new ServiceInvoice()
                        {
                            Id = (Guid)reader["Id"],
                            NumberOfService = (int)reader["NumberOfService"],
                            InvoiceId = (Guid)reader["InvoiceId"],
                            ServiceId = (Guid)reader["ServiceId"],
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
            return new PagedList<IServiceInvoice> (invoiceService,paging.PageNumber,paging.PageSize,itemCount);
        }

        
        public async Task<string> CreateInvoiceServiceAsync(IServiceInvoice newServiceInvoice)
        {

            if (newServiceInvoice == null)
            {
                return "Invoice object is null";
            }
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            int numberOfAffectedRows;

            using (connection)
            {
                string insert = $"INSERT INTO \"InvoiceService\" (\"Id\",\"NumberOfService\",\"InvoiceId\",\"ServiceId\",\"CreatedBy\", \"UpdatedBy\", \"DateCreated\", \"DateUpdated\", \"IsActive\") VALUES (@id,@numOfService,@invoiceId,@serviceId, @createdBy, @updatedBy, @dateCreated, @dateUpdated, true)";
                NpgsqlCommand command = new NpgsqlCommand(insert, connection);
                connection.Open();
                command.Parameters.AddWithValue("id", newServiceInvoice.Id);
                command.Parameters.AddWithValue("numOfService", newServiceInvoice.NumberOfService);
                command.Parameters.AddWithValue("invoiceId", newServiceInvoice.InvoiceId);
                command.Parameters.AddWithValue("serviceId", newServiceInvoice.ServiceId);
                command.Parameters.AddWithValue("createdBy", newServiceInvoice.CreatedBy);
                command.Parameters.AddWithValue("updatedBy", newServiceInvoice.UpdatedBy);
                command.Parameters.AddWithValue("dateCreated", newServiceInvoice.DateCreated);
                command.Parameters.AddWithValue("dateUpdated", newServiceInvoice.DateUpdated);
                numberOfAffectedRows = await command.ExecuteNonQueryAsync();
                connection.Close();
            }

            if (numberOfAffectedRows > 0)
            {
                return ("Invoice added");
            }
            return ("Invoice not added");


        }

        public async Task<int> DeleteAsync(Guid id)
        {
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            using (connection)
            {
                NpgsqlCommand command = new NpgsqlCommand();
                command.CommandText = $"UPDATE \"InvoiceService\" " +
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

        public async Task<PagedList<IServiceInvoiceHistory>> GetServiceInvoiceByInvoiceIdAsync(Guid id,Sorting sorting)
        {
            List<IServiceInvoiceHistory> serviceInvoiceHistories = new List<IServiceInvoiceHistory>();
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            string commandText = $"SELECT \"InvoiceService\".\"Id\", \"Service\".\"Name\", \"Service\".\"Price\", \"InvoiceService\".\"NumberOfService\", \"InvoiceService\".\"DateCreated\" " +
                $"FROM \"InvoiceService\" LEFT JOIN \"Service\" ON \"InvoiceService\".\"ServiceId\" = \"Service\".\"Id\" " +
                $"LEFT JOIN \"Invoice\" ON \"InvoiceService\".\"InvoiceId\" = \"Invoice\".\"Id\" " +
                $" WHERE \"InvoiceId\" = @invoiceId AND \"InvoiceService\".\"IsActive\" = true";
            using (connection)
            {
                NpgsqlCommand command = new NpgsqlCommand();
                command.CommandText = commandText;
                command.Connection = connection;
                command.Parameters.AddWithValue("invoiceId", id);
                if (sorting != null)
                    ApplySorting(command, sorting);

                try
                {
                    await connection.OpenAsync();
                    NpgsqlDataReader reader = await command.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        serviceInvoiceHistories.Add(new ServiceInvoiceHistory()
                        {
                            Id = (Guid)reader["Id"],
                            ServiceName = (string)reader["Name"],
                            Quantity = (int)reader["NumberOfService"],
                            Price = (decimal)reader["Price"],
                            DateCreated = reader.GetDateTime(reader.GetOrdinal("DateCreated"))
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
            return new PagedList<IServiceInvoiceHistory>(serviceInvoiceHistories, 0, 0,serviceInvoiceHistories.Count);
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

        private async Task<int> GetItemCountAsync()
        {
            NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            using (connection)
            {
                NpgsqlCommand command = new NpgsqlCommand();
                command.CommandText = "SELECT COUNT(\"Id\") FROM \"InvoiceService\" WHERE \"IsActive\" = TRUE";
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
