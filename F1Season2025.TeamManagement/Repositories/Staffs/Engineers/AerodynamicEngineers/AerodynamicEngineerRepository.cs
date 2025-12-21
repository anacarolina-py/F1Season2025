using Dapper;
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

    public async Task ChangeAerodynamicEngineerStatusByAerodynamicEngineerIdAsync(int aerodynamicEngineerId, string newStatus)
    {
        try 
        { 
            var sqlUpdateAerodynamicEngineerStatus = @"EXEC sp_ChangeAerodynamicEngineerStatus @AerodynamicEngineerId,@NewStatus;";
            _logger.LogInformation("Changing status of aerodynamic engineer with AerodynamicEngineerId: {AerodynamicEngineerId} to {Status}", aerodynamicEngineerId, newStatus);
            await _connection.ExecuteAsync(sqlUpdateAerodynamicEngineerStatus, new { NewStatus = newStatus, AerodynamicEngineerId = aerodynamicEngineerId });
        }
        catch (SqlException ex)
        {
            _logger.LogError($"SQL Error changing aerodynamic engineer status.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error changing aerodynamic engineer status.");
            throw;
        }
    }

    public async Task CreateAerodynamicEngineerAsync(AerodynamicEngineer aerodynamicEngineer)
    {
        var sqlInsertAerodynamicEngineer = @"EXEC sp_InsertAerodynamicEngineer @FirstName,@LastName,@Age,@Experience,@Status;";

        try { 
            _logger.LogInformation("Creating a new Aerodynamic Engineer in the database.");
            await _connection.ExecuteAsync(sqlInsertAerodynamicEngineer, new
                                                                            {
                                                                                FirstName = aerodynamicEngineer.FirstName,
                                                                                LastName = aerodynamicEngineer.LastName,
                                                                                Age = aerodynamicEngineer.Age,
                                                                                Experience = aerodynamicEngineer.Experience,
                                                                                Status = aerodynamicEngineer.Status
                                                                            });
        }
        catch (SqlException ex)
        {
            _logger.LogError($"SQL Error creating aerodynamic engineer.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error creating aerodynamic engineer.");
            throw;
        }
    }

    public async Task<List<AerodynamicEngineerResponseDTO>> GetActiveAerodynamicEngineersAsync()
    {
        var sqlSelectActiveAerodynamicEngineers = @"SELECT ae.AerodynamicEngineerId,e.EngineerId, s.StaffId, s.FirstName, s.LastName, s.Age, s.Experience, s.Status
                                                    FROM Engineers e
                                                    JOIN Staffs s ON e.StaffId = s.StaffId
                                                    JOIN AerodynamicEngineers ae ON ae.EngineerId = e.EngineerId
                                                    WHERE s.Status = 'Ativo';";
        try 
        { 
            _logger.LogInformation("Retrieving active Aerodynamic Engineers from the database.");
            
            return (await _connection.QueryAsync<AerodynamicEngineerResponseDTO>(sqlSelectActiveAerodynamicEngineers)).ToList();
        }
        catch (SqlException ex)
        {
            _logger.LogError($"SQL Error retrieving active aerodynamic engineers.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving active aerodynamic engineers.");
            throw;
        }
    }

    public async Task<AerodynamicEngineerResponseDTO?> GetAerodynamicEngineerByAerodynamicEngineerIdAsync(int aerodynamicEngineerId)
    {
        var sqlSelectAerodynamicEngineerById = @"SELECT ae.AerodynamicEngineerId,e.EngineerId, s.StaffId, s.FirstName, s.LastName, s.Age, s.Experience, s.Status
                                               FROM Engineers e
                                               JOIN Staffs s ON e.StaffId = s.StaffId
                                               JOIN AerodynamicEngineers ae ON ae.EngineerId = e.EngineerId
                                               WHERE ae.AerodynamicEngineerId = @AerodynamicEngineerId;";

        try 
        { 
            _logger.LogInformation("Retrieving Aerodynamic Engineer by AerodynamicEngineerId from the database.");
            
            return await _connection.QueryFirstOrDefaultAsync<AerodynamicEngineerResponseDTO>(sqlSelectAerodynamicEngineerById, new { AerodynamicEngineerId = aerodynamicEngineerId });
        }
        catch (SqlException ex)
        {
            _logger.LogError($"SQL Error retrieving aerodynamic engineer by AerodynamicEngineerId.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving aerodynamic engineer by AerodynamicEngineerId.");
            throw;
        }
    }

    public async Task<AerodynamicEngineerResponseDTO?> GetAerodynamicEngineerByEngineerIdAsync(int engineerId)
    {
        var sqlSelectAerodynamicEngineerByEngineerId = @"SELECT ae.AerodynamicEngineerId,e.EngineerId, s.StaffId, s.FirstName, s.LastName, s.Age, s.Experience, s.Status
                                                       FROM Engineers e
                                                       JOIN Staffs s ON e.StaffId = s.StaffId
                                                       JOIN AerodynamicEngineers ae ON ae.EngineerId = e.EngineerId
                                                       WHERE e.EngineerId = @EngineerId;";

        try 
        { 
            _logger.LogInformation("Retrieving Aerodynamic Engineer by EngineerId from the database.");
            
            return await _connection.QueryFirstOrDefaultAsync<AerodynamicEngineerResponseDTO>(sqlSelectAerodynamicEngineerByEngineerId, new { EngineerId = engineerId });
        }
        catch (SqlException ex)
        {
            _logger.LogError($"SQL Error retrieving aerodynamic engineer by EngineerId.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving aerodynamic engineer by EngineerId.");
            throw;
        }
    }

    public async Task<AerodynamicEngineerResponseDTO?> GetAerodynamicEngineerByStaffIdAsync(int staffId)
    {
        var sqlSelectAerodynamicEngineerByStaffId = @"SELECT ae.AerodynamicEngineerId,e.EngineerId, s.StaffId, s.FirstName, s.LastName, s.Age, s.Experience, s.Status
                                                   FROM Engineers e
                                                   JOIN Staffs s ON e.StaffId = s.StaffId
                                                   JOIN AerodynamicEngineers ae ON ae.EngineerId = e.EngineerId
                                                   WHERE s.StaffId = @StaffId;";

        try 
        { 
            _logger.LogInformation("Retrieving Aerodynamic Engineer by StaffId from the database.");
            
            return await _connection.QueryFirstOrDefaultAsync<AerodynamicEngineerResponseDTO>(sqlSelectAerodynamicEngineerByStaffId, new { StaffId = staffId });
        }
        catch (SqlException ex)
        {
            _logger.LogError($"SQL Error retrieving aerodynamic engineer by StaffId.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving aerodynamic engineer by StaffId.");
            throw;
        }
    }

    public async Task<List<AerodynamicEngineerResponseDTO>> GetAllAerodynamicEngineersAsync()
    {
        var sqlSelectAllAerodynamicEngineers = @"SELECT ae.AerodynamicEngineerId,e.EngineerId, s.StaffId, s.FirstName, s.LastName, s.Age, s.Experience, s.Status
                                               FROM Engineers e
                                               JOIN Staffs s ON e.StaffId = s.StaffId
                                               JOIN AerodynamicEngineers ae ON ae.EngineerId = e.EngineerId;";

        try 
        { 
            _logger.LogInformation("Retrieving all Aerodynamic Engineers from the database.");
            
            return (await _connection.QueryAsync<AerodynamicEngineerResponseDTO>(sqlSelectAllAerodynamicEngineers)).ToList();
        }
        catch (SqlException ex)
        {
            _logger.LogError($"SQL Error retrieving all aerodynamic engineers.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving all aerodynamic engineers.");
            throw;
        }
    }

    public async Task<List<AerodynamicEngineerResponseDTO>> GetInactiveAerodynamicEngineersAsync()
    {
        var sqlSelectInactiveAerodynamicEngineers = @"SELECT ae.AerodynamicEngineerId,e.EngineerId, s.StaffId, s.FirstName, s.LastName, s.Age, s.Experience, s.Status
                                                    FROM Engineers e
                                                    JOIN Staffs s ON e.StaffId = s.StaffId
                                                    JOIN AerodynamicEngineers ae ON ae.EngineerId = e.EngineerId
                                                    WHERE s.Status = 'Inativo';";
        try
        { 
            _logger.LogInformation("Retrieving inactive Aerodynamic Engineers from the database.");
            
            return (await _connection.QueryAsync<AerodynamicEngineerResponseDTO>(sqlSelectInactiveAerodynamicEngineers)).ToList();
        }
        catch (SqlException ex)
        {
            _logger.LogError($"SQL Error retrieving inactive aerodynamic engineers.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving inactive aerodynamic engineers.");
            throw;
        }
    }
}
