using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Competition.Models.DTOs.Competitions
{
    public class DriverStandingResponseDto
    {
        public int Position { get; set; }
        public string DriverId { get; set; }
        public int Points { get; set; }
        public int Wins { get; set; }
    }
}
