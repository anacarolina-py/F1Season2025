using Bogus;
using Domain.RaceControl.Models.Entities;
using Domain.RaceControl.Models.Entities.Enums;
using FluentAssertions;

namespace F1Season2025.Tests.F1Season2025.Tests.Constructors;

public class SessionTests
{
    [Theory]
    [InlineData(EType.FreePractice1, 1)]
    [InlineData(EType.FreePractice2, 2)]
    [InlineData(EType.FreePractice3, 3)]
    [InlineData(EType.Qualifying, 4)]
    [InlineData(EType.MainRace, 5)]
    public void Constructor_ValidParams_SetsPropertiesCorrectly(EType freePractice, int order)
    {
        var session = new Session(freePractice, order);

        session.Type.Should().Be(freePractice);
        session.Order.Should().Be(order);
    }
}
