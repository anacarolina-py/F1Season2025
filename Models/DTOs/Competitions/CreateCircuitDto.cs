using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Competition.Models.DTOs.Competition
{
    public class CreateCircuitDto
    {
        [Required(ErrorMessage = "The circuit name is required")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "The circuit name contains invalid characters")]
        public string Name { get; init; } = string.Empty;

        [Required(ErrorMessage = "The circuit country is required")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "The circuit country contains invalid characters")]
        public string Country { get; init; } = string.Empty;
        [Range(44, 78)]
        public int Laps { get; init; }
    }
}
