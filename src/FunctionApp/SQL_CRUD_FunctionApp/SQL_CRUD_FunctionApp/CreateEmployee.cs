using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using SQL_CRUD_FunctionApp.Helper;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SQL_CRUD_FunctionApp
{
    public static class CreateEmployee
    {
        [FunctionName("CreateEmployee")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a CreateEmployee request.");

            using (var sqlConn = new SqlConnection(SqlServer.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = sqlConn;
                    cmd.CommandType = CommandType.Text;

                    cmd.CommandText = "INSERT INTO tblEmployee (FirstName, LastName) VALUES (@param1, @param2)";

                    cmd.Parameters.AddWithValue("@param1", $"John_{Guid.NewGuid()}");
                    cmd.Parameters.AddWithValue("@param2", $"Doe_{Guid.NewGuid()}");

                    try
                    {
                        sqlConn.Open();
                        cmd.ExecuteNonQuery();
                    }
                    catch (SqlException e)
                    {
                        log.LogError($"Function CreateEmployee encountered SQL error:: {e.Message}");
                        return new BadRequestObjectResult(e.Message);
                    }

                }
            }

            return new OkObjectResult("Record created.");
        }
    }
}
