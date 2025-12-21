using Bogus;
using Domain.RaceControl.Models.Entities;
using FluentAssertions;

namespace F1Season2025.Tests.F1Season2025.Tests.Constructors;

public class DriverChampionshipTests
{
    private readonly Faker _faker = new("pt_BR");
    [Fact]
    public void Constructor_ValidParams_SetsPropertiesCorrectly()
    {
        var expectedIdDriver = _faker.Random.Guid();
        var expectedNameDriver = _faker.Name.FirstName();
        var expectedNumber = _faker.Random.Number(1, 99);
        var expectedIdTeam = _faker.Random.Number();
        var expectedNameTeam = _faker.Name.JobArea();

        var driver = new DriverChampionship(expectedIdDriver, expectedNameDriver, expectedNumber, expectedIdTeam, expectedNameTeam);

        driver.IdDriver.Should().Be(expectedIdDriver);
        driver.NameDriver.Should().Be(expectedNameDriver);
        driver.Number.Should().Be(expectedNumber);
        driver.IdTeam.Should().Be(expectedIdTeam);
        driver.NameTeam.Should().Be(expectedNameTeam);
    }
}
