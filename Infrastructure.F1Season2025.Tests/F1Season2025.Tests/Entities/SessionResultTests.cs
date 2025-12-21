using Bogus;
using Domain.RaceControl.Models.Entities;
using FluentAssertions;
using MongoDB.Bson;

namespace F1Season2025.Tests.F1Season2025.Tests.Constructors;

public sealed class SessionResultTests
{
    private readonly Faker _faker = new("pt_BR");

    [Fact]
    public void Constructor_ValidParams_SetPropertiesCorrectly()
    {
        var constructors = new List<ConstructorChampionship>();
        var drivers = new List<DriverChampionship>();

        for(int i = 0; i < 10; i++)
        {
            var expectedIdTeam = _faker.Random.Number(1, 11);
            var expectedNameTeam = _faker.Name.JobTitle();

            constructors.Add(new ConstructorChampionship(expectedIdTeam, expectedNameTeam));

            var expectedIdDriver = _faker.Random.Guid();
            var expectedNameDriver = _faker.Name.FirstName();
            var expectedNumber = _faker.Random.Number(1, 99);

            drivers.Add(new DriverChampionship(expectedIdDriver, expectedNameDriver, expectedNumber, expectedIdTeam, expectedNameTeam));
        }

        var sessionResult = new SessionResult(drivers, constructors);

        sessionResult.Id.Should().NotBe(ObjectId.Empty);
        sessionResult.Drivers.Should().NotBeNull();
        sessionResult.Teams.Should().NotBeNull();
    }
}
