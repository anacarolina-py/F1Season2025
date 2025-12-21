using Dapper;
using Domain.TeamManagement.Models.DTOs.Staffs.Drivers;
using Domain.TeamManagement.Models.Entities;
using F1Season2025.TeamManagement.Repositories.Staffs.Drivers.Interfaces;
using Infrastructure.TeamManagement.Data.SQL.Connection;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Net.NetworkInformation;

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

    public async Task ChangeDriverStatusByDriverIdAsync(int driverId, string newStatus)
    {
        try 
        { 
            var sqlUpdateDriverStatus = @"EXEC sp_ChangeDriverStatus @DriverId,@NewStatus;";
            _logger.LogInformation("Changing status of driver with DriverId: {DriverId} to {Status}", driverId, newStatus);
            await _connection.ExecuteAsync(sqlUpdateDriverStatus, new { NewStatus = newStatus, DriverId = driverId });
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex, "SQL Error occurred while changing driver status.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while changing driver status.");
            throw;
        }
    }

    public async Task CreateDriverAsync(Driver driver)
    {
        var sqlInsertDriver = @"EXEC sp_InsertDriver @FirstName,@LastName,@Age,@Experience,@Status,@DriverId,@PerformancePoints,@Handicap;";

        try
        {
            _logger.LogInformation("Creating a new boss: {FirstName} {LastName}", driver.FirstName, driver.LastName);

            await _connection.ExecuteAsync(sqlInsertDriver, new
            {
                FirstName = driver.FirstName,
                LastName = driver.LastName,
                Age = driver.Age,
                Experience = driver.Experience,
                Status = driver.Status,
                DriverId = driver.DriverId,
                PerformancePoints = driver.PerformancePoints,
                Handicap = driver.Handicap
            });
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex, "SQL Error occurred while creating a driver.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating a driver.");
            throw;
        }
    }

    public async Task<List<DriverResponseDTO>> GetActiveDriversAsync()
    {
        var sqlSelectActiveDrivers = @"SELECT d.DriverId, s.StaffId, s.FirstName, s.LastName, s.Age, s.Experience, s.Status
                                       FROM Drivers d
                                       JOIN Staffs s ON d.StaffId = s.StaffId
                                       WHERE s.Status = 'Ativo';";

        try { 
            _logger.LogInformation("Retrieving all active drivers.");

            return (await _connection.QueryAsync<DriverResponseDTO>(sqlSelectActiveDrivers)).ToList();
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex, "SQL Error occurred while retrieving active drivers.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving active drivers.");
            throw;
        }
    }

    public async Task<List<DriverResponseDTO>> GetAllDriversAsync()
    {
        var sqlSelectAllDrivers = @"SELECT d.DriverId, s.StaffId, s.FirstName, s.LastName, s.Age, s.Experience, s.Status
                                   FROM Drivers d
                                   JOIN Staffs s ON d.StaffId = s.StaffId;";

        try
        {
            _logger.LogInformation("Retrieving all drivers.");
            return (await _connection.QueryAsync<DriverResponseDTO>(sqlSelectAllDrivers)).ToList();
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex, "SQL Error occurred while retrieving all drivers.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving all drivers.");
            throw;
        }
    }

    public async Task<DriverResponseDTO?> GetDriverByDriverIdAsync(int driverId)
    {
        var sqlSelectDriverById = @"SELECT d.DriverId, s.StaffId, s.FirstName, s.LastName, s.Age, s.Experience, s.Status
                                   FROM Drivers d
                                   JOIN Staffs s ON d.StaffId = s.StaffId
                                   WHERE d.DriverId = @DriverId;";
        try { 
            _logger.LogInformation("Retrieving driver with ID: {DriverId}", driverId);
            return await _connection.QueryFirstOrDefaultAsync<DriverResponseDTO>(sqlSelectDriverById, new { DriverId = driverId });
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex, "SQL Error occurred while retrieving driver by ID.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving driver by ID.");
            throw;
        }
    }

    public async Task<DriverResponseDTO?> GetDriverByStaffIdAsync(int staffId)
    {
        var sqlSelectDriverByStaffId = @"SELECT d.DriverId, s.StaffId, s.FirstName, s.LastName, s.Age, s.Experience, s.Status
                                   FROM Drivers d
                                   JOIN Staffs s ON d.StaffId = s.StaffId
                                   WHERE s.StaffId = @StaffId;";

        try { 
            _logger.LogInformation("Retrieving driver with Staff ID: {StaffId}", staffId);
            return await _connection.QueryFirstOrDefaultAsync<DriverResponseDTO>(sqlSelectDriverByStaffId, new { StaffId = staffId });
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex, "SQL Error occurred while retrieving driver by Staff ID.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving driver by Staff ID.");
            throw;
        }
    }

    public async Task<List<DriverResponseDTO>> GetInactiveDriversAsync()
    {
        var sqlSelectInactiveDrivers = @"SELECT d.DriverId, s.StaffId, s.FirstName, s.LastName, s.Age, s.Experience, s.Status
                                       FROM Drivers d
                                       JOIN Staffs s ON d.StaffId = s.StaffId
                                       WHERE s.Status = 'Inativo';";

        try { 
            _logger.LogInformation("Retrieving all inactive drivers.");
            return (await _connection.QueryAsync<DriverResponseDTO>(sqlSelectInactiveDrivers)).ToList();
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex, "SQL Error occurred while retrieving inactive drivers.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving inactive drivers.");
            throw;
        }
    }

    //Relacionamento piloto com o time

    public async Task<DriverTeamResponseDTO?> GetDriverTeamRelationshipAsync(int driverId, int teamId)
    {
        var sql = @"SELECT DriverId, TeamId, Status
                  FROM DriversTeams
                  WHERE DriverId = @DriverId
                  AND TeamId = @TeamId;";

        try
        {
            _logger.LogInformation("Retrieving driver-team relationship (DriverId: {DriverId}, TeamId: {TeamId}).",driverId,teamId);

            return await _connection.QueryFirstOrDefaultAsync<DriverTeamResponseDTO>(sql,new { DriverId = driverId, TeamId = teamId });
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex, "SQL error retrieving driver-team relationship.");
            throw;
        }
    }

    public async Task<int> GetActiveDriversCountByTeamIdAsync(int teamId)
    {
        var sql = @"SELECT COUNT(*)
                  FROM DriversTeams
                  WHERE TeamId = @TeamId
                  AND Status = 'Ativo';";

        try
        {
            _logger.LogInformation("Counting active drivers for TeamId {TeamId}.",teamId);

            return await _connection.ExecuteScalarAsync<int>(sql,new { TeamId = teamId });
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex, "SQL error counting active drivers by team.");
            throw;
        }
    }

    public async Task ReactivateDriverTeamRelationshipAsync(int driverId, int teamId)
    {
        var sql = @"UPDATE DriversTeams
                  SET Status = 'Ativo'
                  WHERE DriverId = @DriverId
                  AND TeamId = @TeamId;";

        try
        {
            _logger.LogInformation("Reactivating driver-team relationship (DriverId: {DriverId}, TeamId: {TeamId}).",driverId,teamId);

            await _connection.ExecuteAsync(sql,new { DriverId = driverId, TeamId = teamId });
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex, "SQL error reactivating driver-team relationship.");
            throw;
        }
    }

    public async Task AssignDriverToTeamAsync(int driverId, int teamId)
    {
        var sql = @"INSERT INTO DriversTeams (DriverId, TeamId, Status)
                  VALUES (@DriverId, @TeamId, 'Ativo');";

        try
        {
            _logger.LogInformation("Assigning driver {DriverId} to team {TeamId}.",driverId,teamId);

            await _connection.ExecuteAsync(sql,new { DriverId = driverId, TeamId = teamId });
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex, "SQL error assigning driver to team.");
            throw;
        }
    }
}
