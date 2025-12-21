using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Competition.Models.DTOs.Competitions
{
    public class CircuitResponseDto
    {
        public string Id { get; init; }
        public string CircuitName { get; init; }
        public string Country { get; init; }
        public int Laps { get; init; }
       public bool IsActive { get; init; }
    }
}
