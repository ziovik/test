using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbFetcherConsoleApp
{
    static class DbHelper
    {
        private static readonly string CONNECTION_STR = "Server=localhost;" +
            "Port=5432;" +
            "Database=test_db;" +
            "User Id=postgres;" +
            "Password=root;";

        private static readonly DbConnection connection = new NpgsqlConnection(CONNECTION_STR);

        public static DbConnection GetConnection()
        {
            return connection;
        }
    }
}
