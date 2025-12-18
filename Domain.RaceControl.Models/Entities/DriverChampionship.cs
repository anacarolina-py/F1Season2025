using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Domain.RaceControl.Models.Entities;

public class DriverChampionship
{
    [BsonElement("id_driver")]
    public Guid IdDriver { get; init; }
    [BsonElement("name_driver")]
    public string NameDriver { get; private set; }

    [Range(1, 99)]
    public int Number { get; private set; }
    public int IdTeam { get; private set; }
    public string NameTeam { get; private set; }
    public int Points { get; private set; }
    public int Placing { get; private set; }
    public int Wins { get; private set; }
    [BsonElement("grid_position")]
    public int GridPosition { get; private set; }

    public DriverChampionship(Guid idPilot, string namePilot, int number, int idTeam, string nameTeam)
    {
        IdDriver = idPilot;
        NameDriver = namePilot;
        Number = number;
        IdTeam = idTeam;
        NameTeam = nameTeam;
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
