using Domain.TeamManagement.Models.DTOs.Staffs.Engineers.PowerEngineers;
using Domain.TeamManagement.Models.Entities;
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

    public Task CreatePowerEngineerAsync(PowerEngineer powerEngineer)
    {
        throw new NotImplementedException();
    }

    public Task<List<PowerEngineerResponseDTO>> GetActivePowerEngineersAsync()
    {
        throw new NotImplementedException();
    }

    public Task<List<PowerEngineerResponseDTO>> GetAllPowerEngineersAsync()
    {
        throw new NotImplementedException();
    }

    public Task<List<PowerEngineerResponseDTO>> GetInactivePowerEngineersAsync()
    {
        throw new NotImplementedException();
    }

    public Task<PowerEngineerResponseDTO?> GetPowerEngineerByEngineerIdAsync(int engineerId)
    {
        throw new NotImplementedException();
    }

    public Task<PowerEngineerResponseDTO?> GetPowerEngineerByPowerEngineerIdAsync(int powerEngineerId)
    {
        throw new NotImplementedException();
    }

    public Task<PowerEngineerResponseDTO?> GetPowerEngineerByStaffIdAsync(int staffId)
    {
        throw new NotImplementedException();
    }
}
