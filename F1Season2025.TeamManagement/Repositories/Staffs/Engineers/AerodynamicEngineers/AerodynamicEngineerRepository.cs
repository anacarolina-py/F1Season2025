using F1Season2025.TeamManagement.Repositories.Staffs.Engineers.AerodynamicEngineers.Interfaces;
using Infrastructure.TeamManagement.Data.SQL.Connection;
using Microsoft.Data.SqlClient;

namespace F1Season2025.TeamManagement.Repositories.Staffs.Engineers.AerodynamicEngineers;

public class AerodynamicEngineerRepository : IAerodynamicEngineerRepository
{
    private readonly SqlConnection _connection;
    private readonly ILogger _logger;

    public AerodynamicEngineerRepository(ConnectionDB connection, ILogger<AerodynamicEngineerRepository> logger)
    {
        _connection = connection.GetConnection();
        _logger = logger;
    }
}
