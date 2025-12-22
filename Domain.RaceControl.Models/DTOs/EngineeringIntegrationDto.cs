using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Domain.RaceControl.Models.DTOs
{
    public class EngineeringCarDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; } 

        [JsonPropertyName("aerodynamicCoefficient")]
        public decimal AerodynamicCoefficient { get; set; }

        [JsonPropertyName("powerCoefficient")]
        public decimal PowerCoefficient { get; set; }
    }

    public class EngineeringHandicapDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; } 

        [JsonPropertyName("handicap")]
        public decimal Handicap { get; set; }
    }

    public class CompetitionRaceResultDto
    {
        public string CircuitId { get; set; }
        public List<CompetitionDriverResultDto> Results { get; set; } = new();
    }

    public class CompetitionDriverResultDto
    {
        public int DriverId { get; set; }
        public int Position { get; set; }
        public int Points { get; set; }
        public bool FastestLap { get; set; }
    }
}
