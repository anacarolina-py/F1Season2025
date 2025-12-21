using Domain.TeamManagement.Models.DTOs.Teams;
using Domain.TeamManagement.Models.DTOs.Teams.Relashionships;
using Domain.TeamManagement.Models.Entities;

namespace F1Season2025.TeamManagement.Services.Teams.Interfaces
{
    public interface ITeamService
    {
        Task CreateTeamAsync(TeamRequestDTO teamDTO);

        Task<TeamResponseDTO?> GetTeamByIdAsync(int teamId);

        Task<TeamResponseDTO?> GetTeamByNameAsync(string teamName);

        Task<List<TeamResponseDTO>> GetActiveTeamsAsync();

        Task<List<TeamResponseDTO>> GetInactiveTeamsAsync();

        Task<List<TeamResponseDTO>> GetAllTeamsAsync();

        Task<List<TeamPerformanceResponseDTO>> GetActivePerformanceTeamsAsync();

        Task PrepareTeamByTeamIdAsync(int teamId);

        Task TurnOnTeamByTeamIdAsync(int teamId);

        Task TurnOffTeamByTeamIdAsync(int teamId);

        Task<TeamsValidateResponseDTO> ValidateTeamsAsync();
        Task<IEnumerable<EngineeringInfoDTO>> GetEngineeringInfo(int teamId);

        Task AssignDriverToTeamAsync(int teamId,int driverId);
    }
}
