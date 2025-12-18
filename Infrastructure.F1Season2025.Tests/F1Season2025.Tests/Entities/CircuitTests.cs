using Bogus;
using Domain.RaceControl.Models.Entities;
using FluentAssertions;

namespace F1Season2025.Tests.F1Season2025.Tests.Constructors;

public class CircuitTests
{
    private readonly Faker _faker = new();

    [Fact]
    public void Constructor_ValidParams_SetsPropertiesCorrectly()
    {
        var expectedIdCircuit = _faker.Random.Guid().ToString();
        var expectedNameCircuit = _faker.Name.JobArea();
        var expectedCountry = _faker.Address.Country();
        var expectedLaps = _faker.Random.Number(44, 77);

        var circuit = new Circuit(expectedIdCircuit, expectedNameCircuit, expectedCountry, expectedLaps);

        circuit.IdCircuit.Should().Be(expectedIdCircuit);
        circuit.NameCircuit.Should().Be(expectedNameCircuit);
        circuit.Country.Should().Be(expectedCountry);
        circuit.Laps.Should().Be(expectedLaps);
    }
}
