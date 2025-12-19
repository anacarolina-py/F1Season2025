using Domain.RaceControl.Models.DTOs;
using Domain.RaceControl.Models.Entities;

namespace Domain.RaceControl.Models.Extensions;

public static class RaceControlExtension
{
    public static RaceControlResponseDto ToDto(this RaceGrandPix race)
    {
        if (race is null)
            throw new ArgumentNullException("Race can't be null");

        return new RaceControlResponseDto
        {
            Circuit = race.Circuit,
            Season = race.Season,
            Session = race.Session
        };
    }

    public static RaceGrandPix ToEntity(this RaceControlResponseDto race)
    {
        if (race is null)
            throw new ArgumentNullException("Race can't be null");

        var raceGrandPix = new RaceGrandPix(race.Circuit, race.Season);

        if (race.Session is not null)
            raceGrandPix.SetSession(race.Session.ToList());

        return raceGrandPix;
    }
}
