using Dapper;
using Domain.TeamManagement.Models.DTOs.Cars;
using Domain.TeamManagement.Models.DTOs.Teams;
using Domain.TeamManagement.Models.DTOs.Teams.Relashionships;
using Domain.TeamManagement.Models.Entities;
using F1Season2025.TeamManagement.Repositories.Teams.Interfaces;
using Infrastructure.TeamManagement.Data.SQL.Connection;
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
                await _connection.ExecuteAsync(sqlInsertTeam, new { Name = team.Name , Status = team.Status });
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

        public async Task<List<TeamPerformanceResponseDTO>> GetActivePerformanceTeamsAsync()
        {
            #region SQLQuery
            var sqlSelectAcrivePerformanceTeam = @"SELECT 
                            t.TeamId,
                            bt.BossId,
                            c.CarId,
                            c.AerodynamicCoefficient,
                            c.PowerCoefficient,
                            d.DriverId,
                            d.Handicap,
                            s_d.Experience AS DriverExperience,
                            d.PerformancePoints AS Pd,
                            ae.AerodynamicEngineerId,
                            pe.PowerEngineerId,
                            s_ae.Experience AS AeroExperience,
                            s_pe.Experience AS PowerExperience
                        FROM Teams t
                        JOIN TeamsBosses bt ON t.TeamId = bt.TeamId
                        JOIN TeamsCars tc ON t.TeamId = tc.TeamId
                        JOIN Cars c ON tc.CarId = c.CarId
                        JOIN CarsDrivers cd ON c.CarId = cd.CarId
                        JOIN Drivers d ON cd.DriverId = d.DriverId
                        JOIN Staffs s_d ON d.StaffId = s_d.StaffId
                        JOIN CarsAerodynamic cae ON c.CarId = cae.CarId
                        JOIN AerodynamicEngineers ae ON cae.AerodynamicEngineerId = ae.AerodynamicEngineerId
                        JOIN Engineers e_ae ON ae.EngineerId = e_ae.EngineerId
                        JOIN Staffs s_ae ON e_ae.StaffId = s_ae.StaffId
                        JOIN CarsPower cpe ON c.CarId = cpe.CarId
                        JOIN PowerEngineers pe ON cpe.PowerEngineerId = pe.PowerEngineerId
                        JOIN Engineers e_pe ON pe.EngineerId = e_pe.EngineerId
                        JOIN Staffs s_pe ON e_pe.StaffId = s_pe.StaffId
                        WHERE t.[Status] = 'Ativo' AND
                              bt.[Status] = 'Ativo'  AND 
                              tc.[Status] = 'Ativo' AND 
                              cd.[Status] = 'Ativo';";

            #endregion
            var teamDictionary = new Dictionary<int, TeamPerformanceResponseDTO>();

            try
            {
                var rawData = await _connection.QueryAsync<dynamic>(sqlSelectAcrivePerformanceTeam);
                var rows = rawData.ToList();

                var result = rows.GroupBy(r => (int)r.TeamId)
                                 .Select(teamGroup => new TeamPerformanceResponseDTO
                                  {
                                      TeamId = teamGroup.Key,
                                      BossIds = teamGroup
                                          .Select(r => (int)r.BossId)
                                          .Distinct()
                                          .ToList(),
                                      Cars = teamGroup
                                          .GroupBy(r => (int)r.CarId)
                                          .Select(carGroup => {
                                              var first = carGroup.First();
                                              return new CarPerformanceDTO
                                              {
                                                  CarId = carGroup.Key,
                                                  AerodynamicCoefficient = (decimal)first.AerodynamicCoefficient,
                                                  PowerCoefficient = (decimal)first.PowerCoefficient,
                                                  DriverId = (int)first.DriverId,
                                                  Handicap = (decimal)first.Handicap,
                                                  DriverExperience = (decimal)first.DriverExperience,
                                                  Pd = (decimal)first.Pd,
                                                  AerodynamicEngineerId = (int)first.AerodynamicEngineerId,
                                                  PowerEngineerId = (int)first.PowerEngineerId,
                                                  AeroExperience = (decimal)first.AeroExperience,
                                                  PowerExperience = (decimal)first.PowerExperience
                                              };
                                          }).ToList()
                                  }).ToList();

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching active teams with Dapper grouping");
                throw;
            }
        }

        public async Task PrepareTeamByTeamIdAsync(int teamId)
        {
            try
            { 
                var sqlUpdateTeamStatus = @"UPDATE Teams
                                            SET [Status] = 'Em Preparo'
                                            WHERE TeamId = @TeamId";

                _logger.LogInformation("Changing status of team with TeamId: {TeamId} to 'Em Preparo'", teamId);
                await _connection.ExecuteAsync(sqlUpdateTeamStatus, new {TeamId = teamId });
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "SQL Error changing team status");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error changing team status");
                throw;
            }
        }

        public async Task TurnOnTeamByTeamIdAsync(int teamId)
        {
            try { 
                var sqlUpdateTeamStatus = @"UPDATE Teams
                                            SET [Status] = 'Ativo'
                                            WHERE TeamId = @TeamId";

                _logger.LogInformation("Changing status of team with TeamId: {TeamId} to 'Ativo'", teamId);
                await _connection.ExecuteAsync(sqlUpdateTeamStatus, new { TeamId = teamId });
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "SQL Error changing team status");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error changing team status");
                throw;
            }
        }

        public async Task TurnOffTeamByTeamIdAsync(int teamId)
        {
            try { 
                var sqlUpdateTeamStatus = @"EXEC sp_TurnOffTeam @TeamId";
                _logger.LogInformation("Changing status of team with TeamId: {TeamId} to 'Inativo'", teamId);
                await _connection.ExecuteAsync(sqlUpdateTeamStatus, new { TeamId = teamId });
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "SQL Error changing team status");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error changing team status");
                throw;
            }
        }

        public async Task<int> ValidateTeamsAsync()
        {
            var sqlValidateTeams = @"SELECT COUNT(*) AS CanStart
                                     FROM Teams
                                     WHERE [Status] = 'Ativo'";

            try
            {
                _logger.LogInformation("Validating if there are enough active teams to start the season");
                return await _connection.ExecuteScalarAsync<int>(sqlValidateTeams);
            }
            catch(SqlException ex)
            {
                _logger.LogError(ex, "SQL Error validating teams");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating teams");
                throw;
            }

        }

    }

}
