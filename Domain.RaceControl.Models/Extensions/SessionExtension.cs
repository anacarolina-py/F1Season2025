using Domain.RaceControl.Models.DTOs;
using Domain.RaceControl.Models.Entities;
using Domain.RaceControl.Models.Entities.Enums;

namespace Domain.RaceControl.Models.Extensions;

public static class SessionExtension
{
    public static SessionResponseDto ToDto(this Session session)
    {
        return new SessionResponseDto
        {
            SessionResult = session.SessionResult.ToDto(),
            Status = session.Status.ToString(),
            Type = session.Type.ToString(),
        };
    }
}
