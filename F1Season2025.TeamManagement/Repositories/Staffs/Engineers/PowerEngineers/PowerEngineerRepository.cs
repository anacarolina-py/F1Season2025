using Dapper;
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

    public PowerEngineerRepository(ConnectionDB connection, ILogger<IPowerEngineerRepository> logger)
    {
        _connection = connection.GetConnection();
        _logger = logger;
    }

    public async Task CreatePowerEngineerAsync(PowerEngineer powerEngineer)
    {
        var sqlInsertPowerEngineer = @"EXEC sp_InsertPowerEngineer @FirstName,@LastName,@Age,@Experience,@Status;";

        try
        {
            _logger.LogInformation("Creating a new Power Engineer in the database.");
            await _connection.ExecuteAsync(sqlInsertPowerEngineer, new
            {
                FirstName = powerEngineer.FirstName,
                LastName = powerEngineer.LastName,
                Age = powerEngineer.Age,
                Experience = powerEngineer.Experience,
                Status = powerEngineer.Status
            });
        }
        catch (SqlException ex)
        {
            _logger.LogError($"SQL Error creating power engineer.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error creating power engineer.");
            throw;
        }
    }

    public async Task<List<PowerEngineerResponseDTO>> GetActivePowerEngineersAsync()
    {
        var sqlSelectActivePowerEngineers = @"SELECT ae.PowerEngineerId,e.EngineerId, s.StaffId, s.FirstName, s.LastName, s.Age, s.Experience, s.Status
                                                    FROM Engineers e
                                                    JOIN Staffs s ON e.StaffId = s.StaffId
                                                    JOIN PowerEngineers ae ON ae.EngineerId = e.EngineerId
                                                    WHERE s.Status = 'Ativo';";
        try
        {
            _logger.LogInformation("Retrieving active Power Engineers from the database.");

            return (await _connection.QueryAsync<PowerEngineerResponseDTO>(sqlSelectActivePowerEngineers)).ToList();
        }
        catch (SqlException ex)
        {
            _logger.LogError($"SQL Error retrieving active power engineers.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving active power engineers.");
            throw;
        }
    }

    public async Task<PowerEngineerResponseDTO?> GetPowerEngineerByPowerEngineerIdAsync(int powerEngineerId)
    {
        var sqlSelectPowerEngineerById = @"SELECT ae.PowerEngineerId,e.EngineerId, s.StaffId, s.FirstName, s.LastName, s.Age, s.Experience, s.Status
                                               FROM Engineers e
                                               JOIN Staffs s ON e.StaffId = s.StaffId
                                               JOIN PowerEngineers ae ON ae.EngineerId = e.EngineerId
                                               WHERE ae.PowerEngineerId = @PowerEngineerId;";

        try
        {
            _logger.LogInformation("Retrieving Power Engineer by PowerEngineerId from the database.");

            return await _connection.QueryFirstOrDefaultAsync<PowerEngineerResponseDTO>(sqlSelectPowerEngineerById, new { PowerEngineerId = powerEngineerId });
        }
        catch (SqlException ex)
        {
            _logger.LogError($"SQL Error retrieving power engineer by PowerEngineerId.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving power engineer by PowerEngineerId.");
            throw;
        }
    }

    public async Task<PowerEngineerResponseDTO?> GetPowerEngineerByEngineerIdAsync(int engineerId)
    {
        var sqlSelectPowerEngineerByEngineerId = @"SELECT ae.PowerEngineerId,e.EngineerId, s.StaffId, s.FirstName, s.LastName, s.Age, s.Experience, s.Status
                                                       FROM Engineers e
                                                       JOIN Staffs s ON e.StaffId = s.StaffId
                                                       JOIN PowerEngineers ae ON ae.EngineerId = e.EngineerId
                                                       WHERE e.EngineerId = @EngineerId;";

        try
        {
            _logger.LogInformation("Retrieving Power Engineer by EngineerId from the database.");

            return await _connection.QueryFirstOrDefaultAsync<PowerEngineerResponseDTO>(sqlSelectPowerEngineerByEngineerId, new { EngineerId = engineerId });
        }
        catch (SqlException ex)
        {
            _logger.LogError($"SQL Error retrieving power engineer by EngineerId.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving power engineer by EngineerId.");
            throw;
        }
    }

    public async Task<PowerEngineerResponseDTO?> GetPowerEngineerByStaffIdAsync(int staffId)
    {
        var sqlSelectPowerEngineerByStaffId = @"SELECT ae.PowerEngineerId,e.EngineerId, s.StaffId, s.FirstName, s.LastName, s.Age, s.Experience, s.Status
                                                   FROM Engineers e
                                                   JOIN Staffs s ON e.StaffId = s.StaffId
                                                   JOIN PowerEngineers ae ON ae.EngineerId = e.EngineerId
                                                   WHERE s.StaffId = @StaffId;";

        try
        {
            _logger.LogInformation("Retrieving Power Engineer by StaffId from the database.");

            return await _connection.QueryFirstOrDefaultAsync<PowerEngineerResponseDTO>(sqlSelectPowerEngineerByStaffId, new { StaffId = staffId });
        }
        catch (SqlException ex)
        {
            _logger.LogError($"SQL Error retrieving power engineer by StaffId.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving power engineer by StaffId.");
            throw;
        }
    }

    public async Task<List<PowerEngineerResponseDTO>> GetAllPowerEngineersAsync()
    {
        var sqlSelectAllPowerEngineers = @"SELECT ae.PowerEngineerId,e.EngineerId, s.StaffId, s.FirstName, s.LastName, s.Age, s.Experience, s.Status
                                               FROM Engineers e
                                               JOIN Staffs s ON e.StaffId = s.StaffId
                                               JOIN PowerEngineers ae ON ae.EngineerId = e.EngineerId;";

        try
        {
            _logger.LogInformation("Retrieving all Power Engineers from the database.");

            return (await _connection.QueryAsync<PowerEngineerResponseDTO>(sqlSelectAllPowerEngineers)).ToList();
        }
        catch (SqlException ex)
        {
            _logger.LogError($"SQL Error retrieving all power engineers.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving all power engineers.");
            throw;
        }
    }

    public async Task<List<PowerEngineerResponseDTO>> GetInactivePowerEngineersAsync()
    {
        var sqlSelectInactivePowerEngineers = @"SELECT ae.PowerEngineerId,e.EngineerId, s.StaffId, s.FirstName, s.LastName, s.Age, s.Experience, s.Status
                                                    FROM Engineers e
                                                    JOIN Staffs s ON e.StaffId = s.StaffId
                                                    JOIN PowerEngineers ae ON ae.EngineerId = e.EngineerId
                                                    WHERE s.Status = 'Inativo';";
        try
        {
            _logger.LogInformation("Retrieving inactive Power Engineers from the database.");

            return (await _connection.QueryAsync<PowerEngineerResponseDTO>(sqlSelectInactivePowerEngineers)).ToList();
        }
        catch (SqlException ex)
        {
            _logger.LogError($"SQL Error retrieving inactive power engineers.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving inactive power engineers.");
            throw;
        }
    }
}
