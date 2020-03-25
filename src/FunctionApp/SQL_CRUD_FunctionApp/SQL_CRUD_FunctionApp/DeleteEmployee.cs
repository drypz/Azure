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
    public static class DeleteEmployee
    {
        [FunctionName("DeleteEmployee")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a DeleteEmployee request.");

            using (var sqlConn = new SqlConnection(SqlServer.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = sqlConn;
                    cmd.CommandType = CommandType.Text;

                    //Delete last record in the table
                    cmd.CommandText = "DELETE FROM tblEmployee WHERE Id IN (SELECT TOP 1 Id FROM tblEmployee ORDER BY Id DESC)";

                    try
                    {
                        sqlConn.Open();
                        await cmd.ExecuteNonQueryAsync();
                    }
                    catch (SqlException e)
                    {
                        log.LogError($"Function DeleteEmployee encountered SQL error:: {e.Message}");
                        return new BadRequestObjectResult(e.Message);
                    }

                }
            }

            return new OkObjectResult("Record deleted.");
        }
    }
}
