using Domain.RaceControl.Models.DTOs;
using Domain.RaceControl.Models.Entities;

namespace Domain.RaceControl.Models.Extensions;

public static class RaceControlExtension
{
    public static RaceControlResponseDto ToDto(this RaceGrandPix race)
    {
        return new RaceControlResponseDto
        {
            Circuit = race.Circuit,
            Season = race.Season,
            Session = race.Session
        };
    }
}
