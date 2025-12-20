using Domain.RaceControl.Models.Entities;

namespace Domain.RaceControl.Models.Messages.Commands;

public class CreateRaceCommand
{
    public CircuitRace Circuit { get; init; }
    public Season Season { get; init; }
}
