using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using SQL_CRUD_FunctionApp;
using SQL_CRUD_FunctionApp.Helper;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;

namespace SQL_CRUD_FunctionApp_Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void GetConnectionStringTest()
        {
            var connString = SqlServer.ConnectionString;
        }

        [Test]
        public void Test1()
        {
            using (var conn = new SqlConnection(SqlServer.ConnectionString))
            {
                conn.Open();

            }
        }
    }
}