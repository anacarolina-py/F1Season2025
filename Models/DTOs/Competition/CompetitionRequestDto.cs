using System.ComponentModel.DataAnnotations;

namespace Domain.Competition.Models.DTOs.Competition
{
    public class CompetitionRequestDto
    {
        [Required]
        public Guid CircuitId { get; init; }
        [Required]
        public int Round { get; init; }
    }
}
