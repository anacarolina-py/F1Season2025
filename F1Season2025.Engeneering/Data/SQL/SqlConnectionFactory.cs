using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace F1Season2025.Engineering.Data.SQL
{
    public class SqlConnectionFactory
    {

        private readonly string _connectionString;

        public SqlConnectionFactory(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("SqlServer");
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }


    }
}
