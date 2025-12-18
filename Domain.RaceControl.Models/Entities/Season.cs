using Domain.RaceControl.Models.Entities.Enums;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.RaceControl.Models.Entities;

public class Season
{
    [BsonElement("id_season")]
    public string IdSeason { get; private set; }
    [BsonElement("season_title")]
    public string SeasonTitle { get; private set; }
    

    public Season(string idSeason, string seasonTitle)
    {
        IdSeason = idSeason;
        SeasonTitle = seasonTitle;
    }
}
