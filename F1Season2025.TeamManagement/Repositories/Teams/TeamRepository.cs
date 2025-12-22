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
                await _connection.ExecuteAsync(sqlInsertTeam, new { Name = team.Name, Status = team.Status });
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "SQL Error creating team: {TeamName}", team.Name);
                throw;
            }
            catch (Exception ex)
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
                                          .Select(carGroup =>
                                          {
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

        public async Task TurnOnTeamByTeamIdAsync(int teamId)
        {
            try
            {
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
            try
            {
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
            catch (SqlException ex)
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

        public async Task<IEnumerable<EngineeringInfoDTO>> GetEngineeringInfo(int teamId)
        {
            try
            {
                var sql = @"
                        SELECT
                            t.TeamId,
                            c.CarId,
                            c.AerodynamicCoefficient,
                            c.PowerCoefficient,
                            d.DriverId,
                            d.Handicap AS DriverHandicap,
                            s_d.Experience AS DriverExperience,
                            s_ae.Experience AS EngineerExperienceCa,
                            s_pe.Experience AS EngineerExperienceCp
                            FROM Teams t
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
                        WHERE t.TeamId = @TeamId
                          AND t.[Status] = 'Ativo'
                          AND tc.[Status] = 'Ativo'
                          AND cd.[Status] = 'Ativo'";

                var result = await _connection.QueryAsync<EngineeringInfoDTO>(
                    sql, new
                    {
                        TeamId = teamId
                    });

                return result ?? Enumerable.Empty<EngineeringInfoDTO>();
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "SQL Error retrieving teams");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving teams");
                throw;
            }
        }

        public async Task<TeamDriverResponseDTO?> GetDriverTeamRelationshipAsync(int teamId, int driverId)
        {
            var sql = @"SELECT DriverId, TeamId, Status
                  FROM TeamsDrivers
                  WHERE DriverId = @DriverId
                  AND TeamId = @TeamId;";

            try
            {
                _logger.LogInformation("Retrieving driver-team relationship (DriverId: {DriverId}, TeamId: {TeamId}).", driverId, teamId);

                return await _connection.QueryFirstOrDefaultAsync<TeamDriverResponseDTO>(sql, new { DriverId = driverId, TeamId = teamId });
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
                  FROM TeamsDrivers
                  WHERE TeamId = @TeamId
                  AND Status = 'Ativo';";

            try
            {
                _logger.LogInformation("Counting active drivers for TeamId {TeamId}.", teamId);

                return await _connection.ExecuteScalarAsync<int>(sql, new { TeamId = teamId });
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "SQL error counting active drivers by team.");
                throw;
            }
        }

        public async Task ReactivateDriverTeamRelationshipAsync(int teamId, int driverId)
        {
            var sql = @"UPDATE TeamsDrivers
                  SET Status = 'Ativo'
                  WHERE DriverId = @DriverId
                  AND TeamId = @TeamId;";

            try
            {
                _logger.LogInformation("Reactivating driver-team relationship (DriverId: {DriverId}, TeamId: {TeamId}).", driverId, teamId);

                await _connection.ExecuteAsync(sql, new { DriverId = driverId, TeamId = teamId });
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "SQL error reactivating driver-team relationship.");
                throw;
            }
        }

        public async Task AssignDriverToTeamAsync(int teamId, int driverId)
        {
            var sql = @"INSERT INTO TeamsDrivers (DriverId, TeamId, Status)
                  VALUES (@DriverId, @TeamId, 'Ativo');";

            try
            {
                _logger.LogInformation("Assigning driver {DriverId} to team {TeamId}.", driverId, teamId);

                await _connection.ExecuteAsync(sql, new { DriverId = driverId, TeamId = teamId });
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "SQL error assigning driver to team.");
                throw;
            }
        }


        public async Task<TeamCarResponseDTO?> GetCarTeamRelationshipAsync(int teamId, int carId)
        {
            var sql = @"SELECT CarId, TeamId, Status
                  FROM TeamsCars
                  WHERE CarId = @CarId
                  AND TeamId = @TeamId;";

            try
            {
                _logger.LogInformation("Retrieving car-team relationship (CarId: {CarId}, TeamId: {TeamId}).", carId, teamId);

                return await _connection.QueryFirstOrDefaultAsync<TeamCarResponseDTO>(sql, new { CarId = carId, TeamId = teamId });
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "SQL error retrieving car-team relationship.");
                throw;
            }
        }

        public async Task<int> GetActiveCarsCountByTeamIdAsync(int teamId)
        {
            var sql = @"SELECT COUNT(*)
                  FROM TeamsCars
                  WHERE TeamId = @TeamId
                  AND Status = 'Ativo';";

            try
            {
                _logger.LogInformation("Counting active cars for TeamId {TeamId}.", teamId);

                return await _connection.ExecuteScalarAsync<int>(sql, new { TeamId = teamId });
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "SQL error counting active cars by team.");
                throw;
            }
        }

        public async Task ReactivateCarTeamRelationshipAsync(int teamId, int carId)
        {
            var sql = @"UPDATE TeamsCars
                  SET Status = 'Ativo'
                  WHERE CarId = @CarId
                  AND TeamId = @TeamId;";

            try
            {
                _logger.LogInformation("Reactivating car-team relationship (CarId: {CarId}, TeamId: {TeamId}).", carId, teamId);

                await _connection.ExecuteAsync(sql, new { CarId = carId, TeamId = teamId });
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "SQL error reactivating car-team relationship.");
                throw;
            }
        }

        public async Task AssignCarToTeamAsync(int teamId, int carId)
        {
            var sql = @"INSERT INTO TeamsCars (CarId, TeamId, Status)
                  VALUES (@CarId, @TeamId, 'Ativo');";

            try
            {
                _logger.LogInformation("Assigning car {CarId} to team {TeamId}.", carId, teamId);

                await _connection.ExecuteAsync(sql, new { CarId = carId, TeamId = teamId });
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "SQL error assigning car to team.");
                throw;
            }
        }



        public async Task<TeamBossResponseDTO?> GetBossTeamRelationshipAsync(int teamId, int bossId)
        {
            var sql = @"SELECT BossId, TeamId, Status
                  FROM TeamsBosses
                  WHERE BossId = @BossId
                  AND TeamId = @TeamId;";

            try
            {
                _logger.LogInformation("Retrieving boss-team relationship (BossId: {BossId}, TeamId: {TeamId}).", bossId, teamId);

                return await _connection.QueryFirstOrDefaultAsync<TeamBossResponseDTO>(sql, new { BossId = bossId, TeamId = teamId });
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "SQL error retrieving boss-team relationship.");
                throw;
            }
        }

        public async Task<int> GetActiveBossesCountByTeamIdAsync(int teamId)
        {
            var sql = @"SELECT COUNT(*)
                  FROM TeamsBosses
                  WHERE TeamId = @TeamId
                  AND Status = 'Ativo';";

            try
            {
                _logger.LogInformation("Counting active bosses for TeamId {TeamId}.", teamId);

                return await _connection.ExecuteScalarAsync<int>(sql, new { TeamId = teamId });
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "SQL error counting active bosses by team.");
                throw;
            }
        }

        public async Task ReactivateBossTeamRelationshipAsync(int teamId, int bossId)
        {
            var sql = @"UPDATE TeamsBosses
                  SET Status = 'Ativo'
                  WHERE BossId = @BossId
                  AND TeamId = @TeamId;";

            try
            {
                _logger.LogInformation("Reactivating boss-team relationship (BossId: {BossId}, TeamId: {TeamId}).", bossId, teamId);

                await _connection.ExecuteAsync(sql, new { BossId = bossId, TeamId = teamId });
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "SQL error reactivating boss-team relationship.");
                throw;
            }
        }

        public async Task AssignBossToTeamAsync(int teamId, int bossId)
        {
            var sql = @"INSERT INTO TeamsBosses (BossId, TeamId, Status)
                  VALUES (@BossId, @TeamId, 'Ativo');";

            try
            {
                _logger.LogInformation("Assigning boss {BossId} to team {TeamId}.", bossId, teamId);

                await _connection.ExecuteAsync(sql, new { BossId = bossId, TeamId = teamId });
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "SQL error assigning boss to team.");
                throw;
            }
        }


        public async Task<TeamAerodynamicResponseDTO?> GetAerodynamicEngineerTeamRelationshipAsync(int teamId, int aerodynamicEngineerId)
        {
            var sql = @"SELECT AerodynamicEngineerId, TeamId, Status
                  FROM TeamsAerodynamic
                  WHERE AerodynamicEngineerId = @AerodynamicEngineerId
                  AND TeamId = @TeamId;";

            try
            {
                _logger.LogInformation("Retrieving aerodynamic-engineer-team relationship (AerodynamicEngineerId: {AerodynamicEngineerId}, TeamId: {TeamId}).", aerodynamicEngineerId, teamId);

                return await _connection.QueryFirstOrDefaultAsync<TeamAerodynamicResponseDTO>(sql, new { AerodynamicEngineerId = aerodynamicEngineerId, TeamId = teamId });
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "SQL error retrieving aerodynamic-engineer-team relationship.");
                throw;
            }
        }

        public async Task<int> GetActiveAerodynamicEngineersCountByTeamIdAsync(int teamId)
        {
            var sql = @"SELECT COUNT(*)
                  FROM TeamsAerodynamic
                  WHERE TeamId = @TeamId
                  AND Status = 'Ativo';";

            try
            {
                _logger.LogInformation("Counting active aerodynamic engineers for TeamId {TeamId}.", teamId);

                return await _connection.ExecuteScalarAsync<int>(sql, new { TeamId = teamId });
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "SQL error counting active aerodynamic engineers by team.");
                throw;
            }
        }

        public async Task ReactivateAerodynamicEngineerTeamRelationshipAsync(int teamId, int aerodynamicEngineerId)
        {
            var sql = @"UPDATE TeamsAerodynamic
                  SET Status = 'Ativo'
                  WHERE AerodynamicEngineerId = @AerodynamicEngineerId
                  AND TeamId = @TeamId;";

            try
            {
                _logger.LogInformation("Reactivating aerodynamic-engineer-team relationship (AerodynamicEngineerId: {AerodynamicEngineerId}, TeamId: {TeamId}).", aerodynamicEngineerId, teamId);

                await _connection.ExecuteAsync(sql, new { AerodynamicEngineerId = aerodynamicEngineerId, TeamId = teamId });
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "SQL error reactivating aerodynamic-engineer-team relationship.");
                throw;
            }
        }

        public async Task AssignAerodynamicEngineerToTeamAsync(int teamId, int aerodynamicEngineerId)
        {
            var sql = @"INSERT INTO TeamsAerodynamic (AerodynamicEngineerId, TeamId, Status)
                  VALUES (@AerodynamicEngineerId, @TeamId, 'Ativo');";

            try
            {
                _logger.LogInformation("Assigning aerodynamic-engineer {AerodynamicEngineerId} to team {TeamId}.", aerodynamicEngineerId, teamId);

                await _connection.ExecuteAsync(sql, new { AerodynamicEngineerId = aerodynamicEngineerId, TeamId = teamId });
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "SQL error assigning aerodynamic-engineer to team.");
                throw;
            }
        }




        public async Task<TeamPowerResponseDTO?> GetPowerEngineerTeamRelationshipAsync(int teamId, int powerEngineerId)
        {
            var sql = @"SELECT PowerEngineerId, TeamId, Status
                  FROM TeamsPower
                  WHERE PowerEngineerId = @PowerEngineerId
                  AND TeamId = @TeamId;";

            try
            {
                _logger.LogInformation("Retrieving power-engineer-team relationship (PowerEngineerId: {PowerEngineerId}, TeamId: {TeamId}).", powerEngineerId, teamId);

                return await _connection.QueryFirstOrDefaultAsync<TeamPowerResponseDTO>(sql, new { PowerEngineerId = powerEngineerId, TeamId = teamId });
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "SQL error retrieving power-engineer-team relationship.");
                throw;
            }
        }

        public async Task<int> GetActivePowerEngineersCountByTeamIdAsync(int teamId)
        {
            var sql = @"SELECT COUNT(*)
                  FROM TeamsPower
                  WHERE TeamId = @TeamId
                  AND Status = 'Ativo';";

            try
            {
                _logger.LogInformation("Counting active power engineers for TeamId {TeamId}.", teamId);

                return await _connection.ExecuteScalarAsync<int>(sql, new { TeamId = teamId });
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "SQL error counting active power engineers by team.");
                throw;
            }
        }

        public async Task ReactivatePowerEngineerTeamRelationshipAsync(int teamId, int powerEngineerId)
        {
            var sql = @"UPDATE TeamsPower
                  SET Status = 'Ativo'
                  WHERE PowerEngineerId = @PowerEngineerId
                  AND TeamId = @TeamId;";

            try
            {
                _logger.LogInformation("Reactivating power-engineer-team relationship (PowerEngineerId: {PowerEngineerId}, TeamId: {TeamId}).", powerEngineerId, teamId);

                await _connection.ExecuteAsync(sql, new { PowerEngineerId = powerEngineerId, TeamId = teamId });
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "SQL error reactivating power-engineer-team relationship.");
                throw;
            }
        }

        public async Task AssignPowerEngineerToTeamAsync(int teamId, int powerEngineerId)
        {
            var sql = @"INSERT INTO TeamsPower (PowerEngineerId, TeamId, Status)
                  VALUES (@PowerEngineerId, @TeamId, 'Ativo');";

            try
            {
                _logger.LogInformation("Assigning power-engineer {PowerEngineerId} to team {TeamId}.", powerEngineerId, teamId);

                await _connection.ExecuteAsync(sql, new { PowerEngineerId = powerEngineerId, TeamId = teamId });
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "SQL error assigning power-engineer to team.");
                throw;
            }
        }


    }
}
