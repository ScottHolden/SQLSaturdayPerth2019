using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;

namespace SQLInsert
{
    public class HttpFunctions
    {
		private static SqlConnection SetupConnection()
		{
			string connectionString = Environment.GetEnvironmentVariable("sqldb");

			SqlConnection conn = new SqlConnection(connectionString);

			conn.Open();

			return conn;
		}

		private static SqlCommand PrepareInsert(SqlConnection conn)
		{
			string sql = "INSERT INTO SampleDataTable (data, count) VALUES (@data, 1), (@data, 2), (@data, 3), (@data, 4), (@data, 5), (@data, 6)";

			SqlCommand command = new SqlCommand(sql, conn);
			command.Parameters.AddWithValue("@data", Guid.NewGuid().ToString());

			return command;
		}


		// Setting up individual connections per request

		[FunctionName(nameof(Insert))]
        public static async Task<IActionResult> Insert(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] 
				HttpRequest req,
            ILogger log)
        {
			int rows;
			using (SqlConnection conn = SetupConnection())
			{
				using (SqlCommand cmd = PrepareInsert(conn))
				{
					rows = await cmd.ExecuteNonQueryAsync();
				}
			}

			return new OkObjectResult(rows);
		}
    }
}
