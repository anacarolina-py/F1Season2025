using Domain.TeamManagement.Models.DTOs.Staffs.Engineers.AerodynamicEngineers;
using Domain.TeamManagement.Models.Entities;
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

    public Task CreateAerodynamicEngineerAsync(AerodynamicEngineer aerodynamicEngineer)
    {
        throw new NotImplementedException();
    }

    public Task<List<AerodynamicEngineerResponseDTO>> GetActiveAerodynamicEngineersAsync()
    {
        throw new NotImplementedException();
    }

    public Task<AerodynamicEngineerResponseDTO?> GetAerodynamicEngineerByAerodynamicEngineerIdAsync(int aerodynamicEngineerId)
    {
        throw new NotImplementedException();
    }

    public Task<AerodynamicEngineerResponseDTO?> GetAerodynamicEngineerByEngineerIdAsync(int engineerId)
    {
        throw new NotImplementedException();
    }

    public Task<AerodynamicEngineerResponseDTO?> GetAerodynamicEngineerByStaffIdAsync(int staffId)
    {
        throw new NotImplementedException();
    }

    public Task<List<AerodynamicEngineerResponseDTO>> GetAllAerodynamicEngineersAsync()
    {
        throw new NotImplementedException();
    }

    public Task<List<AerodynamicEngineerResponseDTO>> GetInactiveAerodynamicEngineersAsync()
    {
        throw new NotImplementedException();
    }
}
