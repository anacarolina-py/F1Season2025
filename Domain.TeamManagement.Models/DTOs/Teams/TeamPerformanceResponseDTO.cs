using Domain.TeamManagement.Models.DTOs.Cars;

namespace Domain.TeamManagement.Models.DTOs.Teams;

public class TeamPerformanceResponseDTO
{
    public int TeamId { get; init; }
    public List<int> BossIds { get; init; }
    public List<CarPerformanceDTO> Cars { get; init; }
}
