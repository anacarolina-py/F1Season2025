using Domain.RaceControl.Models.DTOs;
using Domain.RaceControl.Models.Entities;

namespace Domain.RaceControl.Models.Extensions;

public static class SeasonExtension
{
    public static Season ToEntity(this SeasonResponseDto season)
    {
        return new Season(
            season.IdSeason, 
            season.SeasonTitle
            );
    }

    public static SeasonResponseDto ToDto(this Season season)
    {
        return new SeasonResponseDto
        {
            IdSeason = season.IdSeason,
            SeasonTitle = season.SeasonTitle
        };
    }
}
