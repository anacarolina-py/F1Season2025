using Domain.RaceControl.Models.Entities;

namespace Domain.RaceControl.Models.Messages.Commands;

public class CreateRaceCommand
{
    public Circuit Circuit { get; init; }
    public Season Season { get; init; }
}
