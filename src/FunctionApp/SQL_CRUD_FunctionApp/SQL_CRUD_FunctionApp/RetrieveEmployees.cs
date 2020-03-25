using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using SQL_CRUD_FunctionApp.Helper;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SQL_CRUD_FunctionApp
{
    public static class RetrieveEmployees
    {
        [FunctionName("RetrieveEmployees")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a RetrieveEmployees request.");
            var employees = new List<Employee>();

            using (var sqlConn = new SqlConnection(SqlServer.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = sqlConn;
                    cmd.CommandType = CommandType.Text;

                    cmd.CommandText = "SELECT [Id] ,[FirstName] ,[LastName] FROM [TestDB].[dbo].[tblEmployee]";

                    try
                    {
                        sqlConn.Open();

                        var reader = await cmd.ExecuteReaderAsync();

                        while (reader.Read())
                        {
                            employees.Add(new Employee
                            {
                                Id = (int)reader["Id"],
                                FirstName = reader["FirstName"].ToString(),
                                LastName = reader["LastName"].ToString()
                            });
                        }
                    }
                    catch (SqlException e)
                    {
                        log.LogError($"Function RetrieveEmployees encountered SQL error:: {e.Message}");
                        return new BadRequestObjectResult(e.Message);
                    }

                }
            }

            return new OkObjectResult(employees);
        }
    }
}
