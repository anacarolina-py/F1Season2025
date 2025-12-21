using Domain.TeamManagement.Models.DTOs.Teams;
using Domain.TeamManagement.Models.DTOs.Teams.Relashionships;
using Domain.TeamManagement.Models.Entities;

namespace F1Season2025.TeamManagement.Repositories.Teams.Interfaces
{
    public interface ITeamRepository
    {
        Task<TeamResponseDTO?> GetTeamByNameAsync(string teamName);

        Task CreateTeamAsync(Team team);

        Task<TeamResponseDTO?> GetTeamByIdAsync(int teamId);

        Task<List<TeamResponseDTO>> GetActiveTeamsAsync();

        Task<List<TeamPerformanceResponseDTO>> GetActivePerformanceTeamsAsync();

        Task<List<TeamResponseDTO>> GetInactiveTeamsAsync();

        Task<List<TeamResponseDTO>> GetAllTeamsAsync();
        Task PrepareTeamByTeamIdAsync(int teamId);
        Task TurnOnTeamByTeamIdAsync(int teamId);
        Task TurnOffTeamByTeamIdAsync(int teamId);
        Task<int> ValidateTeamsAsync();
        Task<IEnumerable<EngineeringInfoDTO>> GetEngineeringInfo(int teamId);

        Task<TeamDriverResponseDTO?> GetDriverTeamRelationshipAsync(int teamId, int driverId);
        Task<int> GetActiveDriversCountByTeamIdAsync(int teamId);
        Task ReactivateDriverTeamRelationshipAsync(int teamId, int driverId);
        Task AssignDriverToTeamAsync(int teamId, int driverId);
    }
}
