using System.ComponentModel.DataAnnotations;

namespace Domain.TeamManagement.Models.DTOs.Teams;

public class TeamRequestDTO
{
    [Required(ErrorMessage = "The team name is required")]
    public string Name { get; init; }
}
