using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Competition.Models.DTOs.Competition
{
    public class CreateCircuitDto
    {
        [Required]
        public string Name { get; init; } = string.Empty;
        [Required]
        public string Country { get; init; } = string.Empty;
        [Range(1, 78)]
        public int Laps { get; init; }
    }
}
