using MongoDB.Bson.Serialization.Attributes;

namespace Domain.RaceControl.Models.Entities;

public class ConstructorChampionship
{
    [BsonElement("id_team")]
    public int IdTeam { get; private set; }
    [BsonElement("name_team")]
    public string NameTeam { get; private set; }
    [BsonElement("total_points")]
    public int TotalPoints { get; private set; }
    public int Placing { get; private set; }
    public int Wins { get; private set; }

    public ConstructorChampionship(int idTeam, string nameTeam)
    {
        IdTeam = idTeam;
        NameTeam = nameTeam;
    }

    public void SetTotalPoints(int points)
    {
        TotalPoints += points; 
    }

    public void SetPlacing (int placing)
    {
        Placing = placing;
        if(placing == 1)
        {
            Wins += 1;
        }
    }

}
