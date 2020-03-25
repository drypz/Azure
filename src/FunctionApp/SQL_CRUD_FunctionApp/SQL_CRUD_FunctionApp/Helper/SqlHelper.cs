using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Reflection;

namespace SQL_CRUD_FunctionApp.Helper
{
    public static class SqlServer
    {
        public static string ConnectionString => ResolveConnectionString();

        private static string ResolveConnectionString()
        {
            var connString = Environment.GetEnvironmentVariable("sqldb_connection");

            if (string.IsNullOrEmpty(connString))
            {
                var executionPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

                var config = new ConfigurationBuilder()
                    .SetBasePath(executionPath)
                    .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables()
                    .Build();

                connString = config.GetConnectionString("sqldb_connection");
            }

            return connString;
        }
    }
}
