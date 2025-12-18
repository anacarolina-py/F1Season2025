using F1Season2025.TeamManagement.Repositories.Staffs.Engineers.PowerEngineers.Interfaces;
using Infrastructure.TeamManagement.Data.SQL.Connection;
using Microsoft.Data.SqlClient;

namespace F1Season2025.TeamManagement.Repositories.Staffs.Engineers.PowerEngineers;

public class PowerEngineerRepository : IPowerEngineerRepository
{
    private readonly SqlConnection _connection;
    private readonly ILogger _logger;

    public PowerEngineerRepository(ConnectionDB connection, ILogger<PowerEngineerRepository> logger)
    {
        _connection = connection.GetConnection();
        _logger = logger;
    }
}
