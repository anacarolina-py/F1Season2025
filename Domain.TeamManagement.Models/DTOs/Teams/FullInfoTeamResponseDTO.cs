using Domain.TeamManagement.Models.DTOs.Cars.Relashionships;
using Domain.TeamManagement.Models.DTOs.Staffs.Bosses;

namespace Domain.TeamManagement.Models.DTOs.Teams;

public class FullInfoTeamResponseDTO
{
    public int TeamId { get; init; } 

    public string Name { get; init; }

    public List<FullInfoResponseDTO> Bosses { get; init; }

    public List<FullInfoCarResponseDTO> Cars { get; init; }
}
