using Domain.RaceControl.Models.DTOs;
using Domain.RaceControl.Models.Entities;

namespace Domain.RaceControl.Models.Extensions;

public static class ConstructorExtension
{
    public static ConstructorChampionshipResponseDto ToDto(this ConstructorChampionship constructor)
    {
        return new ConstructorChampionshipResponseDto
        {
            NameTeam = constructor.NameTeam,
            Placing = constructor.Placing,
            TotalPoints = constructor.TotalPoints,
            Wins = constructor.Wins
        };
    }
}
