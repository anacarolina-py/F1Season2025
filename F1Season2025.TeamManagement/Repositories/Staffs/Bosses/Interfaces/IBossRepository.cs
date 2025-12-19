using Domain.TeamManagement.Models.DTOs.Staffs.Bosses;
using Domain.TeamManagement.Models.Entities;

namespace F1Season2025.TeamManagement.Repositories.Staffs.Bosses.Interfaces;

public interface IBossRepository
{
    Task CreateBossAsync(Boss boss);

    Task<BossResponseDTO?> GetBossByBossIdAsync(int bossId);

    Task<BossResponseDTO?> GetBossByStaffIdAsync(int staffId);

    Task<List<BossResponseDTO>> GetAllBossesAsync();

    Task<List<BossResponseDTO>> GetActiveBossesAsync();

    Task<List<BossResponseDTO>> GetInactiveBossesAsync();
}
