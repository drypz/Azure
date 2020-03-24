using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Data.SqlClient;
using SQL_CRUD_FunctionApp.Helper;
using System.Data;

namespace SQL_CRUD_FunctionApp
{
    public static class RetrieveEmployees
    {
        [FunctionName("RetrieveEmployees")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a RetrieveEmployees request.");

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
