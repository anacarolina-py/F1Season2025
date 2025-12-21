using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Domain.RaceControl.Models.Entities;

public class DriverChampionship
{
    [BsonElement("id_driver")]
    public int IdDriver { get; init; }

    [BsonElement("name_driver")]
    public string NameDriver { get; private set; }

    [Range(1, 99)]
    [BsonElement("number")]
    public int Number { get; private set; }

    [BsonElement("id_team")]
    public int IdTeam { get; private set; }

    [BsonElement("name_team")]
    public string NameTeam { get; private set; }

    [BsonElement("performance_points")]
    public decimal PerformancePoints { get; private set; }

    [BsonElement("handicap")]
    public decimal Handicap { get; private set; }

    [BsonElement("points")]
    public int Points { get; private set; }

    [BsonElement("placing")]
    public int Placing { get; private set; }

    [BsonElement("wins")]
    public int Wins { get; private set; }

    [BsonElement("grid_position")]
    public int GridPosition { get; private set; }

    public DriverChampionship(int idPilot, string namePilot, int number, int idTeam, string nameTeam, decimal performancePoints, decimal handicap)
    {
        IdDriver = idPilot;
        NameDriver = namePilot;
        Number = number;
        IdTeam = idTeam;
        NameTeam = nameTeam;
        PerformancePoints = performancePoints;
        Handicap = handicap;
    }

    public void SetPoints (int points)
    {
        Points = points;
    }

    public void SetPlacing (int placing)
    {
        Placing = placing;
        if (placing == 1)
        {
            Wins += 1;
        }
    }

    public void SetGridPosition (int gridPosition)
    {
        GridPosition = gridPosition;
    }
}
