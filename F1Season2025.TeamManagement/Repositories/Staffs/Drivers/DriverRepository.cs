using F1Season2025.TeamManagement.Repositories.Staffs.Drivers.Interfaces;
using Infrastructure.TeamManagement.Data.SQL.Connection;
using Microsoft.Data.SqlClient;

namespace F1Season2025.TeamManagement.Repositories.Staffs.Drivers;

public class DriverRepository : IDriverRepository
{
    private readonly SqlConnection _connection;
    private readonly ILogger _logger;

    public DriverRepository(ConnectionDB connection, ILogger<DriverRepository> logger)
    {
        _connection = connection.GetConnection();
        _logger = logger;
    }
}
