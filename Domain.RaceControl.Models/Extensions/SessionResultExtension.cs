using Domain.RaceControl.Models.DTOs;
using Domain.RaceControl.Models.Entities;

namespace Domain.RaceControl.Models.Extensions;

public static class SessionResultExtension
{
    public static SessionResultResponseDto ToDto(this SessionResult sessionResult)
    {
        if (sessionResult is null)
            return null;

        return new SessionResultResponseDto
        {
            Drivers = [.. sessionResult.Drivers.Select(d => d.ToDto())],
            Teams = [.. sessionResult.Teams.Select(t => t.ToDto())]
        };
    }
}
