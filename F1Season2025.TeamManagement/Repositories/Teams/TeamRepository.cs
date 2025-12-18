using Dapper;
using Domain.TeamManagement.Models.DTOs.Teams;
using Domain.TeamManagement.Models.Entities;
using F1Season2025.TeamManagement.Repositories.Teams.Interfaces;
using Infrastructure.TeamManagement.Data.SQL.Connection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace F1Season2025.TeamManagement.Repositories.Teams
{
    public class TeamRepository : ITeamRepository
    {
        private readonly SqlConnection _connection;
        private readonly ILogger _logger;

        public TeamRepository(ConnectionDB connection, ILogger<TeamRepository> logger)
        {
            _connection = connection.GetConnection();
            _logger = logger;
        }

        public async Task CreateTeamAsync(Team team)
        {
            var sqlInsertTeam = @"INSERT INTO Teams([Name],[Status])
                                  VALUES(@Name,@Status)";

            try
            {
                _logger.LogInformation("Creating a new team: {TeamName}", team.Name);
                await _connection.ExecuteAsync(sqlInsertTeam, new { Name = team.Name , Status = "Inativo" });
            }
            catch(SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "SQL Error creating team: {TeamName}", team.Name);
                throw;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error creating team: {TeamName}", team.Name);
                throw;
            }
        }

        public async Task<List<TeamResponseDTO>> GetActiveTeamsAsync()
        {
            var sqlSelectInactiveTeams = @"SELECT [TeamId], [Name], [Status] 
                                           FROM Teams 
                                           WHERE [Status] = @Status
                                           ORDER BY [Name] ASC";

            try
            {
                return (await _connection.QueryAsync<TeamResponseDTO>(sqlSelectInactiveTeams, new { Status = "Ativo" })).ToList();
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "SQL Error retrieving active teams");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving active teams");
                throw;
            }
        }

        public async Task<List<TeamResponseDTO>> GetAllTeamsAsync()
        {
            var sqlSelectAllTeams = @"SELECT [TeamId], [Name], [Status] 
                                      FROM Teams
                                      ORDER BY[Name] ASC";

            try
            {
                return (await _connection.QueryAsync<TeamResponseDTO>(sqlSelectAllTeams)).ToList();
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "SQL Error retrieving all teams");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all teams");
                throw;
            }
        }

        public async Task<List<TeamResponseDTO>> GetInactiveTeamsAsync()
        {
            var sqlSelectInactiveTeams = @"SELECT [TeamId], [Name], [Status] 
                                           FROM Teams 
                                           WHERE [Status] = @Status
                                           ORDER BY [Name] ASC";

            try
            {
                return (await _connection.QueryAsync<TeamResponseDTO>(sqlSelectInactiveTeams, new { Status = "Inativo" })).ToList();
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "SQL Error retrieving inactive teams");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving inactive teams");
                throw;
            }
        }

        public async Task<TeamResponseDTO?> GetTeamByIdAsync(int teamId)
        {
            var sqlSelectTeamById = @"SELECT [TeamId], [Name], [Status] 
                                        FROM Teams 
                                        WHERE [TeamId] = @TeamId";

            try
            {
                return await _connection.QueryFirstOrDefaultAsync<TeamResponseDTO>(sqlSelectTeamById, new { TeamId = teamId });
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "SQL Error retrieving team by id: {TeamId}", teamId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving team by id: {TeamId}", teamId);
                throw;
            }
        }

        public async Task<TeamResponseDTO?> GetTeamByNameAsync(string teamName)
        {
            var sqlSelectTeamByName = @"SELECT [TeamId], [Name], [Status] 
                                        FROM Teams 
                                        WHERE Name = @Name";

            try
            {
                return await _connection.QueryFirstOrDefaultAsync<TeamResponseDTO>(sqlSelectTeamByName, new { Name = teamName });
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "SQL Error retrieving team by name: {TeamName}", teamName);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving team by name: {TeamName}", teamName);
                throw;
            }
            
        }
    }
}
