using Domain.TeamManagement.Models.DTOs.Bosses;
using Microsoft.AspNetCore.Mvc;

namespace F1Season2025.TeamManagement.Services.Staffs.Bosses.Interfaces;

public interface IBossService
{
    Task CreateBossAsync(BossRequestDTO bossDTO);

    Task<BossResponseDTO?> GetBossByBossIdAsync(int bossId);

    Task<BossResponseDTO?> GetBossByStaffIdAsync(int staffId);

    Task<List<BossResponseDTO>> GetAllBossesAsync();

    Task<List<BossResponseDTO>> GetActiveBossesAsync();

    Task<List<BossResponseDTO>> GetInactiveBossesAsync();
}
