using System.ComponentModel.DataAnnotations;

namespace Domain.Competition.Models.DTOs.Competition
{
    public class CompetitionRequestDto
    {
        [Required]
        public string CircuitId { get; init; } = string.Empty;
    }
}
