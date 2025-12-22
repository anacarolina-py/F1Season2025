using Dapper;
using Domain.TeamManagement.Models.DTOs.Staffs.Drivers;
using Domain.TeamManagement.Models.DTOs.Teams.Relashionships;
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
        var sqlSelectActiveDrivers = @"SELECT d.DriverId, d.Handicap,d.PerformancePoints,
                                              s.StaffId, s.FirstName, s.LastName, s.Age, s.Experience, s.Status,
                                              td.TeamId
                                       FROM Drivers d
                                       JOIN Staffs s ON d.StaffId = s.StaffId
                                       LEFT JOIN TeamsDrivers td ON td.DriverId = d.DriverId
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
        var sqlSelectAllDrivers = @"SELECT d.DriverId,d.Handicap,d.PerformancePoints, 
                                           s.StaffId, s.FirstName, s.LastName, s.Age, s.Experience, s.Status,
                                           td.TeamId
                                   FROM Drivers d
                                   JOIN Staffs s ON d.StaffId = s.StaffId
                                   LEFT JOIN TeamsDrivers td ON td.DriverId = d.DriverId;";

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
        var sqlSelectDriverById = @"SELECT d.DriverId, d.Handicap,d.PerformancePoints,
                                           s.StaffId, s.FirstName, s.LastName, s.Age, s.Experience, s.Status,
                                           td.TeamId
                                   FROM Drivers d
                                   JOIN Staffs s ON d.StaffId = s.StaffId
                                   LEFT JOIN TeamsDrivers td ON td.DriverId = d.DriverId
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
        var sqlSelectDriverByStaffId = @"SELECT d.DriverId, d.Handicap,d.PerformancePoints,
                                                s.StaffId, s.FirstName, s.LastName, s.Age, s.Experience, s.Status,
                                                td.TeamId
                                         FROM Drivers d
                                         JOIN Staffs s ON d.StaffId = s.StaffId
                                         LEFT JOIN TeamsDrivers td ON td.DriverId = d.DriverId
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
        var sqlSelectInactiveDrivers = @"SELECT d.DriverId, d.Handicap,d.PerformancePoints,
                                                s.StaffId, s.FirstName, s.LastName, s.Age, s.Experience, s.Status,
                                                td.TeamId
                                       FROM Drivers d
                                       JOIN Staffs s ON d.StaffId = s.StaffId
                                       LEFT JOIN TeamsDrivers td ON td.DriverId = d.DriverId
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

}
