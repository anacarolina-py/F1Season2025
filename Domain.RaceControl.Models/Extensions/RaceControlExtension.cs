using Domain.RaceControl.Models.DTOs;
using Domain.RaceControl.Models.Entities;

namespace Domain.RaceControl.Models.Extensions;

public static class RaceControlExtension
{
    public static RaceControlResponseDto ToDto(this RaceGrandPix race)
    {
        if (race is null)
            return null;

        return new RaceControlResponseDto
        {
            Circuit = race.Circuit.ToDto(),
            Season = race.Season.ToDto(),
            Session = [.. race.Session.Select(s => s.ToDto())]
        };
    }
}
