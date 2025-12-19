using Domain.TeamManagement.Models.DTOs.Teams;

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
    }
}
