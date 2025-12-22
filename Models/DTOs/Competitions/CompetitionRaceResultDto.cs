using Domain.RaceControl.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Competition.Models.DTOs.Competitions
{
    public class CompetitionRaceResultDto
    {
        public string CircuitId { get; set; }
        public List<CompetitionDriverResultDto> Results { get; set; } = new();
    }

    public class CompetitionDriverResultDto
    {
        public string DriverId { get; set; }
        public int Position { get; set; }
        public int Points { get; set; }
    }
}
