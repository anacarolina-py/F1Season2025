using Bogus;
using Domain.RaceControl.Models.Entities;
using FluentAssertions;

namespace F1Season2025.Tests.F1Season2025.Tests.Constructors;

public class ConstructorChampionshipTests
{
    private readonly Faker _faker = new("pt_BR");

    [Fact]
    public void Constructor_ValidParams_SetPropertiesCorrectly()
    {
        var expectedIdTeam = _faker.Random.Number(1, 11);
        var expectedNameTeam = _faker.Name.JobTitle();

        var constructor = new ConstructorChampionship(expectedIdTeam, expectedNameTeam);

        constructor.IdTeam.Should().Be(expectedIdTeam);
        constructor.NameTeam.Should().Be(expectedNameTeam);
    }
}
