using Domain.TeamManagement.Models.DTOs.Teams;
using Domain.TeamManagement.Models.Entities;

namespace F1Season2025.TeamManagement.Repositories.Teams.Interfaces
{
    public interface ITeamRepository
    {
        Task<TeamResponseDTO?> GetTeamByNameAsync(string teamName);

        Task CreateTeamAsync(Team team);

        Task<TeamResponseDTO?> GetTeamByIdAsync(int teamId);

        Task<List<TeamResponseDTO>> GetActiveTeamsAsync();

        Task<List<TeamResponseDTO>> GetInactiveTeamsAsync();

        Task<List<TeamResponseDTO>> GetAllTeamsAsync();
    }
}
