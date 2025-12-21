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


        Task<TeamCarResponseDTO?> GetCarTeamRelationshipAsync(int teamId, int carId);
        Task<int> GetActiveCarsCountByTeamIdAsync(int teamId);
        Task ReactivateCarTeamRelationshipAsync(int teamId, int carId);
        Task AssignCarToTeamAsync(int teamId, int carId);


        Task<TeamBossResponseDTO?> GetBossTeamRelationshipAsync(int teamId, int bossId);
        Task<int> GetActiveBossesCountByTeamIdAsync(int teamId);
        Task ReactivateBossTeamRelationshipAsync(int teamId, int bossId);
        Task AssignBossToTeamAsync(int teamId, int bossId);


        Task<TeamAerodynamicResponseDTO?> GetAerodynamicEngineerTeamRelationshipAsync(int teamId, int aerodynamicEngineerId);
        Task<int> GetActiveAerodynamicEngineersCountByTeamIdAsync(int teamId);
        Task ReactivateAerodynamicEngineerTeamRelationshipAsync(int teamId, int aerodynamicEngineerId);
        Task AssignAerodynamicEngineerToTeamAsync(int teamId, int aerodynamicEngineerId);


        Task<TeamPowerResponseDTO?> GetPowerEngineerTeamRelationshipAsync(int teamId, int powerEngineerId);
        Task<int> GetActivePowerEngineersCountByTeamIdAsync(int teamId);
        Task ReactivatePowerEngineerTeamRelationshipAsync(int teamId, int powerEngineerId);
        Task AssignPowerEngineerToTeamAsync(int teamId, int powerEngineerId);
    }
}
