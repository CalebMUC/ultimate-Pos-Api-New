using Microsoft.EntityFrameworkCore;
// using Ultimate_POS_Api.Data;
// using Ultimate_POS_Api.DTOS;
// using Ultimate_POS_Api.Models;
// using MySql.Data.MySqlClient;
using System.Data;
using System.Threading.Tasks;
// using System.Data;

using Newtonsoft.Json;
//using MySqlConnector;
using Ultimate_POS_Api.DTOS;
using Ultimate_POS_Api.Models;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Text.Json;
using Ultimate_POS_Api.DTOS.Reports;

namespace Ultimate_POS_Api.Repository
{

    public class ReportRepository : IReportRepository
    {

        private readonly IConfiguration _configuration;

        public ReportRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        //public async Task<DataSet> GetReportData(string reportType, Dictionary<string, string> parameters)
        //{
        //    // Create a new DataSet to store the result
        //    DataSet dataSet = new DataSet();

        //    // Replace "DefaultConnection" with your MySQL connection string name
        //    string connectionString = _configuration.GetConnectionString("DefaultConnection");

        //    using (MySqlConnection connection = new MySqlConnection(connectionString))
        //    {
        //        await connection.OpenAsync();

        //        // Prepare the stored procedure call
        //        using (MySqlCommand command = new MySqlCommand($"Get{reportType.Replace(" ", "")}", connection))
        //        {
        //            command.CommandType = CommandType.StoredProcedure;

        //            // Add parameters to the command
        //            foreach (var param in parameters)
        //            {
        //                command.Parameters.AddWithValue($"@{param.Key}", param.Value);
        //            }

        //            // Use MySqlDataAdapter to fill the DataSet
        //            using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
        //            {
        //                adapter.Fill(dataSet); // Fills the DataSet with the result of the query
        //            }
        //        }
        //    }

        //    return dataSet;
        //}

        //   public Task<IEnumerable<Supplier>> GetReportData()
        //   {
        //       throw new NotImplementedException();
        //   }

        // public async Task<DataSeIReportRepositoryt> GetReportData(string reportType, Dictionary<string, string> parameters)
        // {
        //     // Create a new DataSet to store the result
        //     DataSet dataSet = new DataSet();

        //     using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
        //     {
        //         await connection.OpenAsync();

        //         using (SqlCommand command = new SqlCommand($"Get{reportType.Replace(" ", "")}", connection))
        //         {
        //             command.CommandType = CommandType.StoredProcedure;

        //             // Add parameters to the command
        //             foreach (var param in parameters)
        //             {
        //                 command.Parameters.AddWithValue($"@{param.Key}", param.Value);
        //             }

        //             // Use SqlDataAdapter to fill the DataSet
        //             using (SqlDataAdapter adapter = new SqlDataAdapter(command))
        //             {
        //                 adapter.Fill(dataSet); // Fills the DataSet with the result of the query
        //             }
        //         }
        //     }

        //     return dataSet;
        // }

        public async Task<DataSet> GetLowStockReportAsync(LowStockReportDto lowStockReportDto)
        {
            // This method is intended to generate a low stock report.
            var functionName = "get_stock_alert_report";
            var dataSet = new DataSet();
            var dataTable = new DataTable();
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var jsonParams = JsonConvert.SerializeObject(lowStockReportDto);
            try
            {
                await using var connection = new NpgsqlConnection(connectionString);
                await connection.OpenAsync();
                var query = $"SELECT * FROM {functionName}(@params)";
                await using var command = new NpgsqlCommand(query, connection);
                command.Parameters.AddWithValue("params", NpgsqlTypes.NpgsqlDbType.Jsonb,jsonParams);

                await using var reader = await command.ExecuteReaderAsync();
                dataTable.Load(reader);
                dataSet.Tables.Add(dataTable);

                return dataSet;

            }
            catch (Exception ex)
            {
                throw new Exception("Error generating low stock report", ex);
            }
        }



        public async Task<DataSet> GetReportData(ReportRequestData reportRequestData)
        {
            var dataSet = new DataSet();
            var dataTable = new DataTable();

            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            await using var connection = new NpgsqlConnection(connectionString);
            await connection.OpenAsync();

            var functionName = $"get_{reportRequestData.ReportType.Replace(" ", "_").ToLower()}";
            var sql = $"SELECT * FROM {functionName}(@params)";

            await using var command = new NpgsqlCommand(sql, connection);

            // Flatten parameters (handle JsonElement cases)
            var cleanedParams = reportRequestData.parameters.ToDictionary(
                kvp => kvp.Key,
                kvp => ConvertJsonElement(kvp.Value)
            );

            var jsonParams = JsonConvert.SerializeObject(cleanedParams);

            command.Parameters.AddWithValue("params", NpgsqlTypes.NpgsqlDbType.Jsonb, jsonParams);

            await using var reader = await command.ExecuteReaderAsync();
            dataTable.Load(reader);
            dataSet.Tables.Add(dataTable);

            return dataSet;
        }




        // Helper method to clean JsonElements and other wrapped types
        public static object ConvertJsonElement(object value)
        {
            if (value is JsonElement element)
            {
                switch (element.ValueKind)
                {
                    case JsonValueKind.Number:
                        return element.TryGetInt64(out var l) ? (object)l : element.GetDecimal();
                    case JsonValueKind.String:
                        return element.GetString();
                    case JsonValueKind.True:
                    case JsonValueKind.False:
                        return element.GetBoolean();
                    case JsonValueKind.Null:
                    case JsonValueKind.Undefined:
                        return null;
                    case JsonValueKind.Object:
                    case JsonValueKind.Array:
                        return element.ToString(); // Nested objects as raw JSON
                }
            }

            return value; // Already a primitive
        }






    }

}