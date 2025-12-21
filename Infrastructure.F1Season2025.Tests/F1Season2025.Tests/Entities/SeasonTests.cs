using Bogus;
using Domain.RaceControl.Models.Entities;
using FluentAssertions;

namespace F1Season2025.Tests.F1Season2025.Tests.Constructors;

public class SeasonTests
{
    private readonly Faker _faker = new();

    [Fact]
    public void Constructor_ValidParams_SetsPropertiesCorrectly()
    {
        var expectedId = _faker.Random.Guid().ToString();
        var expectedTitle = _faker.Name.JobTitle();

        var season = new Season(expectedId, expectedTitle);

        season.IdSeason.Should().Be(expectedId);
        season.SeasonTitle.Should().Be(expectedTitle);
    }

}
