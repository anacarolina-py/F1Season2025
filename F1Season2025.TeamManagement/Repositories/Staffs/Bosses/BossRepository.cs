using Dapper;
using Domain.TeamManagement.Models.DTOs.Staffs.Bosses;
using Domain.TeamManagement.Models.Entities;
using F1Season2025.TeamManagement.Repositories.Staffs.Bosses.Interfaces;
using Infrastructure.TeamManagement.Data.SQL.Connection;
using Microsoft.Data.SqlClient;

namespace F1Season2025.TeamManagement.Repositories.Staffs.Bosses;

public class BossRepository : IBossRepository
{
    private readonly SqlConnection _connection;
    private readonly ILogger _logger;

    public BossRepository(ConnectionDB connection, ILogger<BossRepository> logger)
    {
        _connection = connection.GetConnection();
        _logger = logger;
    }

    public async Task ChangeBossStatusByBossIdAsync(int bossId, string newStatus)
    {
        try { 
            var sqlUpdateBossStatus = "EXEC sp_ChangeBossStatus @BossId,@NewStatus";
            _logger.LogInformation("Changing status of boss with BossId: {BossId} to {Status}", bossId, newStatus);
            await _connection.ExecuteAsync(sqlUpdateBossStatus, new { NewStatus = newStatus, BossId = bossId });
        }
        catch (SqlException sqlEx)
        {
            _logger.LogError(sqlEx, "SQL Error changing boss status");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error changing boss status");
            throw;
        }
    }

    public async Task CreateBossAsync(Boss boss)
    {
        var sqlInsertBoss = "EXEC sp_InsertBoss @FirstName, @LastName, @Age, @Experience, @Status";
        try
        {
            _logger.LogInformation("Creating a new boss: {FirstName} {LastName}", boss.FirstName, boss.LastName);
            await _connection.ExecuteAsync(sqlInsertBoss, new
                                                             {
                                                                 FirstName = boss.FirstName,
                                                                 LastName = boss.LastName,
                                                                 Age = boss.Age,
                                                                 Experience = boss.Experience,
                                                                 Status = boss.Status
                                                             });
        }
        catch(SqlException sqlEx)
        {
            _logger.LogError(sqlEx, "SQL Error creating boss");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating boss");
            throw;
        }
    }

    public async Task<List<BossResponseDTO>> GetActiveBossesAsync()
    {
        var sqlSelectActiveBosses = @"SELECT b.BossId, s.StaffId, s.FirstName, s.LastName, s.Age, s.Experience, s.Status
                                       FROM Bosses b
                                       JOIN Staffs s ON b.StaffId = s.StaffId
                                       WHERE s.Status = 'Ativo';";

        try { 
            _logger.LogInformation("Retrieving all active bosses");

            return (await _connection.QueryAsync<BossResponseDTO>(sqlSelectActiveBosses)).ToList();
        }
        catch (SqlException sqlEx)
        {
            _logger.LogError(sqlEx, "SQL Error retrieving active bosses");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving active bosses");
            throw;
        }
    }

    public async Task<List<BossResponseDTO>> GetAllBossesAsync()
    {
        var sqlSelectAllBosses = @"SELECT b.BossId, s.StaffId, s.FirstName, s.LastName, s.Age, s.Experience, s.Status
                                       FROM Bosses b
                                       JOIN Staffs s ON b.StaffId = s.StaffId;";

        try { 
            _logger.LogInformation("Retrieving all bosses");
            return (await _connection.QueryAsync<BossResponseDTO>(sqlSelectAllBosses)).ToList();
        }
        catch (SqlException sqlEx)
        {
            _logger.LogError(sqlEx, "SQL Error retrieving all bosses");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all bosses");
            throw;
        }
    }

    public async Task<BossResponseDTO?> GetBossByBossIdAsync(int bossId)
    {
        var sqlSelectBossByBossId = @"SELECT b.BossId, s.StaffId, s.FirstName, s.LastName, s.Age, s.Experience, s.Status
                                       FROM Bosses b
                                       JOIN Staffs s ON b.StaffId = s.StaffId
                                       WHERE b.BossId = @BossId;";
        try { 
            _logger.LogInformation("Retrieving boss with BossId: {BossId}", bossId);
            return await _connection.QueryFirstOrDefaultAsync<BossResponseDTO>(sqlSelectBossByBossId, new { BossId = bossId });
        }
        catch (SqlException sqlEx)
        {
            _logger.LogError(sqlEx, "SQL Error retrieving boss by BossId");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving boss by BossId");
            throw;
        }
    }

    public async Task<BossResponseDTO?> GetBossByStaffIdAsync(int staffId)
    {
        var sqlSelectBossByStaffId = @"SELECT b.BossId, s.StaffId, s.FirstName, s.LastName, s.Age, s.Experience, s.Status
                                       FROM Bosses b
                                       JOIN Staffs s ON b.StaffId = s.StaffId
                                       WHERE s.StaffId = @StaffId;";

        try { 
            _logger.LogInformation("Retrieving boss with StaffId: {StaffId}", staffId);
            return await _connection.QueryFirstOrDefaultAsync<BossResponseDTO>(sqlSelectBossByStaffId, new { StaffId = staffId });
        }
        catch (SqlException sqlEx)
        {
            _logger.LogError(sqlEx, "SQL Error retrieving boss by StaffId");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving boss by StaffId");
            throw;
        }
    }

    public async Task<List<BossResponseDTO>> GetInactiveBossesAsync()
    {
        var sqlSelectInactiveBosses = @"SELECT b.BossId, s.StaffId, s.FirstName, s.LastName, s.Age, s.Experience, s.Status
                                       FROM Bosses b
                                       JOIN Staffs s ON b.StaffId = s.StaffId
                                       WHERE s.Status = 'Inativo';";

        try
        {
            _logger.LogInformation("Retrieving all inactive bosses");
            return (await _connection.QueryAsync<BossResponseDTO>(sqlSelectInactiveBosses)).ToList();
        }
        catch (SqlException sqlEx)
        {
            _logger.LogError(sqlEx, "SQL Error retrieving inactive bosses");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving inactive bosses");
            throw;
        }
    }


}
