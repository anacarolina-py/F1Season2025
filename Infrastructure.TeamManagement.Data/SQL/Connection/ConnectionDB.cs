using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.TeamManagement.Data.SQL.Connection;

public class ConnectionDB
{
    private readonly string _connectionString;

    public ConnectionDB(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");

    }

    public SqlConnection GetConnection()
    {
        return new SqlConnection(_connectionString);
    }
}
