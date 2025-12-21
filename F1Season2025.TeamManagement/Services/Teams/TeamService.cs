using Domain.TeamManagement.Models.DTOs.Teams;
using Domain.TeamManagement.Models.DTOs.Teams.Relashionships;
using Domain.TeamManagement.Models.Entities;
using F1Season2025.TeamManagement.Repositories.Teams.Interfaces;
using F1Season2025.TeamManagement.Services.Teams.Interfaces;
using Microsoft.AspNetCore.Mvc;

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

        public async Task CreateTeamAsync(TeamRequestDTO teamDTO)
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

                await _teamRepository.CreateTeamAsync(newTeam);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a team.");
                throw;
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
    }
}
