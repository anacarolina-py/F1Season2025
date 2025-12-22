using Domain.RaceControl.Models.DTOs;
using Domain.RaceControl.Models.Entities;

namespace Domain.RaceControl.Models.Extensions;

public static class DriverExtension
{
    public static DriverChampionshipResponseDto ToDto(this DriverChampionship driver)
    {
        if (driver is null)
            throw new ArgumentNullException();

        return new DriverChampionshipResponseDto
        {
            NameDriver = driver.NameDriver,
            Number = driver.Number,
            GridPosition = driver.GridPosition,
            NameTeam = driver.NameTeam,
            Placing = driver.Placing,
            Points = driver.Points,
            Wins = driver.Wins
        };
    }
}
