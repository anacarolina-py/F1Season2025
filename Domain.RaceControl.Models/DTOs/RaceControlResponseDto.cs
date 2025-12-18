using Domain.RaceControl.Models.Entities;

namespace Domain.RaceControl.Models.DTOs;

public class RaceControlResponseDto
{
    public Circuit Circuit { get; init; }
    public Season Season { get; init; }
    public IReadOnlyCollection<Session> Session { get; init; } = [];
}
