using Domain.TeamManagement.Models.DTOs.Teams;
using Domain.TeamManagement.Models.DTOs.Teams.Relashionships;
using Domain.TeamManagement.Models.Entities;
using F1Season2025.TeamManagement.Repositories.Teams.Interfaces;
using F1Season2025.TeamManagement.Services.Teams.Interfaces;
using Microsoft.AspNetCore.Connections;
using Microsoft.Data.SqlClient;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace F1Season2025.TeamManagement.Services.Teams
{
    public class TeamService : ITeamService
    {
        private readonly ITeamRepository _teamRepository;
        private readonly ILogger _logger;

        public TeamService(ITeamRepository teamRepository, ILogger<TeamService> logger)
        {
            _teamRepository = teamRepository;
            _logger = logger;
        }

        public async Task ProduceTeamAsync(TeamRequestDTO teamDTO)
        {

            #region Validation
            if (string.IsNullOrEmpty(teamDTO.Name))
            {
                _logger.LogWarning("Attempted to create a team with an invalid name.");
                throw new ArgumentException("Team name cannot be null or empty.");
            }

            if (teamDTO.Name.Length > 255)
            {
                _logger.LogWarning("Attempted to create a team with a name exceeding maximum length: {TeamName}", teamDTO.Name);
                throw new ArgumentException("Team name cannot exceed 255 characters.");
            }

            if (teamDTO.Name.Length < 3)
            {
                _logger.LogWarning("Attempted to create a team with a name below minimum length: {TeamName}", teamDTO.Name);
                throw new ArgumentException("Team name must be at least 3 characters long.");
            }

            #endregion

            try
            {
                _logger.LogInformation("Creating a new team: {TeamName}", teamDTO.Name);

                var teamExists = await _teamRepository.GetTeamByNameAsync(teamDTO.Name);

                if (teamExists is not null)
                {
                    _logger.LogWarning("Attempted to create a team that already exists: {TeamName}", teamDTO.Name);
                    throw new InvalidOperationException($"Team with name {teamDTO.Name} already exists.");
                }

                var newTeam = new Team(teamDTO.Name);

                var factory = new ConnectionFactory { HostName = "localhost" };
                using var connection = await factory.CreateConnectionAsync();
                using var channel = await connection.CreateChannelAsync();

                await channel.QueueDeclareAsync
                    (
                        queue: "TeamQueue",
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null
                    );

                var teamString = JsonSerializer.Serialize<Team>(newTeam);
                var teamByteArray = Encoding.UTF8.GetBytes(teamString);

                await channel.BasicPublishAsync
                    (
                        exchange: string.Empty,
                        routingKey: "TeamQueue",
                        body: teamByteArray
                    );
            }
            catch (SqlException ex)
            {
                _logger.LogError($"Error creating team: {ex.Message}");
                throw;
            }
            catch(InvalidOperationException ex)
            {
                _logger.LogError(ex, "Error creating team:", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating team", ex);
            }
        }


        public async Task ConsumeTeamAsync()
        {
            try
            {
                var factory = new ConnectionFactory { HostName = "localhost" };
                using var connection = await factory.CreateConnectionAsync();

                using var channel = await connection.CreateChannelAsync();

                await channel.QueueDeclareAsync
                (
                    queue: "TeamQueue",
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null
                );

                var consumer = new AsyncEventingBasicConsumer(channel);

                consumer.ReceivedAsync += async (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = System.Text.Encoding.UTF8.GetString(body);
                    var team = JsonSerializer.Deserialize<Team>(message);

                    await _teamRepository.CreateTeamAsync(team);
                };

                await channel.BasicConsumeAsync
                (
                    queue: "TeamQueue",
                    autoAck: true,
                    consumer: consumer
                );

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while consuming teams from queue." + ex.Message);
            }
        }

        public async Task<List<TeamResponseDTO>> GetActiveTeamsAsync()
        {
            try
            {
                _logger.LogInformation("Searching for active teams.");
                return await _teamRepository.GetActiveTeamsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while searching for active teams.");
                throw;
            }
        }

        public async Task<List<TeamResponseDTO>> GetAllTeamsAsync()
        {
            try
            {
                _logger.LogInformation("Searching for inactive teams.");
                return await _teamRepository.GetAllTeamsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while searching for inactive teams.");
                throw;
            }
        }

        public async Task<List<TeamResponseDTO>> GetInactiveTeamsAsync()
        {
            try
            {
                _logger.LogInformation("Searching for all teams.");
                return await _teamRepository.GetInactiveTeamsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while searching for all teams.");
                throw;
            }
        }

        public async Task<TeamResponseDTO?> GetTeamByIdAsync(int teamId)
        {
            try
            {
                _logger.LogInformation($"Searching for {teamId}'s team.");
                return await _teamRepository.GetTeamByIdAsync(teamId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while searching for a team.");
                throw;
            }
        }

        public async Task<TeamResponseDTO?> GetTeamByNameAsync(string teamName)
        {
            try
            {
                _logger.LogInformation($"Searching for {teamName}'s team.");
                return await _teamRepository.GetTeamByNameAsync(teamName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while searching for a team.");
                throw;
            }
        }

        public async Task<List<TeamPerformanceResponseDTO>> GetActivePerformanceTeamsAsync()
        {
            try
            {
                _logger.LogInformation("Searching for active performance teams.");
                return await _teamRepository.GetActivePerformanceTeamsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while searching for active performance teams.");
                throw;
            }
        }


        public async Task PrepareTeamByTeamIdAsync(int teamId)
        {
            try
            {
                _logger.LogInformation("Changing team status by Id in the database.");

                var team = await _teamRepository.GetTeamByIdAsync(teamId);
                if (team is null)
                {
                    _logger.LogWarning("Team with Id {TeamId} not found.", teamId);
                    throw new KeyNotFoundException($"Team with Id {teamId} not found.");
                }

                if (team.Status is "Em Preparo")
                {
                    _logger.LogWarning("Attempted to prepare an already prepared team with Id: {TeamId}", teamId);
                    throw new InvalidOperationException($"Cannot prepare an already prepared team with Id {teamId}.");
                }

                await _teamRepository.PrepareTeamByTeamIdAsync(teamId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while changing team status by Id.");
                throw;
            }
        }

        public async Task TurnOnTeamByTeamIdAsync(int teamId)
        {
            try
            {
                _logger.LogInformation("Turning on team by Id in the database.");
                var team = await _teamRepository.GetTeamByIdAsync(teamId);

                if (team is null)
                {
                    _logger.LogWarning("Team with Id {TeamId} not found.", teamId);
                    throw new KeyNotFoundException($"Team with Id {teamId} not found.");
                }

                if (team.Status is "Inativo")
                {
                    _logger.LogWarning("Attempted to turn on an inactive team with Id: {TeamId}", teamId);
                    throw new InvalidOperationException($"Cannot turn on an inactive team with Id {teamId}.");
                }
                if (team.Status is "Ativo")
                {
                    _logger.LogWarning("Attempted to turn on an already active team with Id: {TeamId}", teamId);
                    throw new InvalidOperationException($"Cannot turn on an already active team with Id {teamId}.");
                }

                await _teamRepository.TurnOnTeamByTeamIdAsync(teamId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while turning on team by Id.");
                throw;
            }
        }

        public async Task TurnOffTeamByTeamIdAsync(int teamId)
        {
            try
            {
                _logger.LogInformation("Turning off team by Id in the database.");
                var team = await _teamRepository.GetTeamByIdAsync(teamId);

                if (team is null)
                {
                    _logger.LogWarning("Team with Id {TeamId} not found.", teamId);
                    throw new KeyNotFoundException($"Team with Id {teamId} not found.");
                }
                if (team.Status is "Inativo")
                {
                    _logger.LogWarning("Attempted to turn off an already inactive team with Id: {TeamId}", teamId);
                    throw new InvalidOperationException($"Cannot turn off an already inactive team with Id {teamId}.");
                }

                await _teamRepository.TurnOffTeamByTeamIdAsync(teamId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while turning off team by Id.");
                throw;
            }
        }

        public async Task<TeamsValidateResponseDTO> ValidateTeamsAsync()
        {
            try
            {
                _logger.LogInformation("Validating teams.");
                var activeTeams = await _teamRepository.ValidateTeamsAsync();

                var teamsValidateResponseDTO = new TeamsValidateResponseDTO();
                if (activeTeams is not 11)
                {
                    return teamsValidateResponseDTO;
                }
                teamsValidateResponseDTO.SetCanStart(true);
                return teamsValidateResponseDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while validating teams.");
                throw;
            }
        }

         public async Task<IEnumerable<EngineeringInfoDTO>> GetEngineeringInfo(int teamId)
        {
            try
            {
                _logger.LogInformation("Getting information from teams.");
                return await _teamRepository.GetEngineeringInfo(teamId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving information from teams.");
                throw;
            }

        }


        public async Task AssignDriverToTeamAsync(int teamId,int driverId)
        {
            try
            {
                _logger.LogInformation("Assigning driver with DriverId {DriverId} to team with TeamId {TeamId}.", driverId, teamId);

                var relationship = await _teamRepository.GetDriverTeamRelationshipAsync(teamId,driverId);

                if (relationship is not null)
                {

                    if (await _teamRepository.GetActiveDriversCountByTeamIdAsync(teamId) > 1)
                    {
                        _logger.LogWarning("Team {TeamId} already has the maximum number of active drivers.", teamId);

                        throw new InvalidOperationException($"Team {teamId} already has 2 active drivers.");
                    }

                    if (relationship.Status is "Ativo")
                    {
                        _logger.LogInformation("Driver {DriverId} is already assigned to team {TeamId}.", driverId, teamId);

                        throw new InvalidOperationException($"Driver {driverId} is already assigned to team {teamId}.");
                    }

                    await _teamRepository.ReactivateDriverTeamRelationshipAsync(teamId, driverId);

                }
                else 
                {
                    await _teamRepository.AssignDriverToTeamAsync(teamId, driverId);
                }
                    
            }
            catch (SqlException ex)
            {
                _logger.LogError( ex,"SQL error occurred while assigning driver {DriverId} to team {TeamId}.",driverId,teamId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"An error occurred while assigning driver {DriverId} to team {TeamId}.",driverId,teamId);
                throw;
            }
        }



        public async Task AssignCarToTeamAsync(int teamId, int carId)
        {
            try
            {
                _logger.LogInformation("Assigning car with CarId {CarId} to team with TeamId {TeamId}.", carId, teamId);

                var relationship = await _teamRepository.GetCarTeamRelationshipAsync(teamId, carId);

                if (relationship is not null)
                {

                    if (await _teamRepository.GetActiveCarsCountByTeamIdAsync(teamId) > 1)
                    {
                        _logger.LogWarning("Team {TeamId} already has the maximum number of active cars.", teamId);

                        throw new InvalidOperationException($"Team {teamId} already has 2 active cars.");
                    }

                    if (relationship.Status is "Ativo")
                    {
                        _logger.LogInformation("Car {CarId} is already assigned to team {TeamId}.", carId, teamId);

                        throw new InvalidOperationException($"Car {carId} is already assigned to team {teamId}.");
                    }

                    await _teamRepository.ReactivateCarTeamRelationshipAsync(teamId, carId);

                }
                else
                {
                    await _teamRepository.AssignCarToTeamAsync(teamId, carId);
                }

            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "SQL error occurred while assigning car {CarId} to team {TeamId}.", carId, teamId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while assigning car {CarId} to team {TeamId}.", carId, teamId);
                throw;
            }
        }

        public async Task AssignBossToTeamAsync(int teamId, int bossId)
        {
            try
            {
                _logger.LogInformation("Assigning boss with BossId {BossId} to team with TeamId {TeamId}.", bossId, teamId);

                var relationship = await _teamRepository.GetBossTeamRelationshipAsync(teamId, bossId);

                if (relationship is not null)
                {

                    if (await _teamRepository.GetActiveBossesCountByTeamIdAsync(teamId) > 1)
                    {
                        _logger.LogWarning("Team {TeamId} already has the maximum number of active bosss.", teamId);

                        throw new InvalidOperationException($"Team {teamId} already has 2 active bosss.");
                    }

                    if (relationship.Status is "Ativo")
                    {
                        _logger.LogInformation("Boss {BossId} is already assigned to team {TeamId}.", bossId, teamId);

                        throw new InvalidOperationException($"Boss {bossId} is already assigned to team {teamId}.");
                    }

                    await _teamRepository.ReactivateBossTeamRelationshipAsync(teamId, bossId);

                }
                else
                {
                    await _teamRepository.AssignBossToTeamAsync(teamId, bossId);
                }

            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "SQL error occurred while assigning boss {BossId} to team {TeamId}.", bossId, teamId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while assigning boss {BossId} to team {TeamId}.", bossId, teamId);
                throw;
            }
        }

        public async Task AssignAerodynamicEngineerToTeamAsync(int teamId, int aerodynamicEngineerId)
        {
            try
            {
                _logger.LogInformation("Assigning aerodynamic engineer with AerodynamicEngineerId {AerodynamicEngineerId} to team with TeamId {TeamId}.", aerodynamicEngineerId, teamId);

                var relationship = await _teamRepository.GetAerodynamicEngineerTeamRelationshipAsync(teamId, aerodynamicEngineerId);

                if (relationship is not null)
                {

                    if (await _teamRepository.GetActiveAerodynamicEngineersCountByTeamIdAsync(teamId) > 1)
                    {
                        _logger.LogWarning("Team {TeamId} already has the maximum number of active aerodynamic engineers.", teamId);

                        throw new InvalidOperationException($"Team {teamId} already has 2 active aerodynamic engineers.");
                    }

                    if (relationship.Status is "Ativo")
                    {
                        _logger.LogInformation("AerodynamicEngineer {AerodynamicEngineerId} is already assigned to team {TeamId}.", aerodynamicEngineerId, teamId);

                        throw new InvalidOperationException($"AerodynamicEngineer {aerodynamicEngineerId} is already assigned to team {teamId}.");
                    }

                    await _teamRepository.ReactivateAerodynamicEngineerTeamRelationshipAsync(teamId, aerodynamicEngineerId);

                }
                else
                {
                    await _teamRepository.AssignAerodynamicEngineerToTeamAsync(teamId, aerodynamicEngineerId);
                }

            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "SQL error occurred while assigning aerodynamic engineer {AerodynamicEngineerId} to team {TeamId}.", aerodynamicEngineerId, teamId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while assigning aerodynamic engineer {AerodynamicEngineerId} to team {TeamId}.", aerodynamicEngineerId, teamId);
                throw;
            }
        }
        public async Task AssignPowerEngineerToTeamAsync(int teamId, int powerEngineerId)
        {
            try
            {
                _logger.LogInformation("Assigning power engineer with PowerEngineerId {PowerEngineerId} to team with TeamId {TeamId}.", powerEngineerId, teamId);

                var relationship = await _teamRepository.GetPowerEngineerTeamRelationshipAsync(teamId, powerEngineerId);

                if (relationship is not null)
                {

                    if (await _teamRepository.GetActivePowerEngineersCountByTeamIdAsync(teamId) > 1)
                    {
                        _logger.LogWarning("Team {TeamId} already has the maximum number of active power engineers.", teamId);

                        throw new InvalidOperationException($"Team {teamId} already has 2 active power engineers.");
                    }

                    if (relationship.Status is "Ativo")
                    {
                        _logger.LogInformation("PowerEngineer {PowerEngineerId} is already assigned to team {TeamId}.", powerEngineerId, teamId);

                        throw new InvalidOperationException($"PowerEngineer {powerEngineerId} is already assigned to team {teamId}.");
                    }

                    await _teamRepository.ReactivatePowerEngineerTeamRelationshipAsync(teamId, powerEngineerId);

                }
                else
                {
                    await _teamRepository.AssignPowerEngineerToTeamAsync(teamId, powerEngineerId);
                }

            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "SQL error occurred while assigning power engineer {PowerEngineerId} to team {TeamId}.", powerEngineerId, teamId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while assigning power engineer {PowerEngineerId} to team {TeamId}.", powerEngineerId, teamId);
                throw;
            }
        }

        public async Task<IEnumerable<FullInfoTeamResponseDTO>> GetFullInfoTeams()
        {
            try
            {
                _logger.LogInformation("Searching for teams.");
                return await _teamRepository.GetFullInfoTeams();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while searching for teams.");
                throw;
            }
        }

    }
}
