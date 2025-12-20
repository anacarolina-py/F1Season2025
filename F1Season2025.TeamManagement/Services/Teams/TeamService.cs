using Domain.TeamManagement.Models.DTOs.Teams;
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
    }
}
