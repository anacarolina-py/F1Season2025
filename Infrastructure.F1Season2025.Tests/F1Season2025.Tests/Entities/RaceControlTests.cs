using Bogus;
using Domain.RaceControl.Models.Entities;
using Domain.RaceControl.Models.Entities.Enums;
using FluentAssertions;

namespace F1Season2025.Tests.F1Season2025.Tests.Constructors;

public class RaceControlTests
{
    private readonly Faker _faker = new("pt_BR");

    [Fact]
    public void Constructor_ValidParams_SetsPropertiesCorrectly()
    {
        var expectedIdCircuit = _faker.Random.Guid().ToString();
        var expectedNameCircuit = _faker.Name.JobArea();
        var expectedCountry = _faker.Address.Country();
        var expectedLaps = _faker.Random.Number(44, 77);

        var circuit = new Circuit(expectedIdCircuit, expectedNameCircuit, expectedCountry, expectedLaps);

        var expectedId = _faker.Random.Guid().ToString();
        var expectedTitle = _faker.Name.JobTitle();

        var season = new Season(expectedId, expectedTitle);

        var raceControl = new RaceGrandPix(circuit, season);

        raceControl.Season.Should().Be(season);
        raceControl.Circuit.Should().Be(circuit);
    }

    [Fact]
    public void StartSession_ValidSequence_ChangesStatusToLive()
    {
        var expectedIdCircuit = _faker.Random.Guid().ToString();
        var expectedNameCircuit = _faker.Name.JobArea();
        var expectedCountry = _faker.Address.Country();
        var expectedLaps = _faker.Random.Number(44, 77);

        var circuit = new Circuit(expectedIdCircuit, expectedNameCircuit, expectedCountry, expectedLaps);

        var expectedId = _faker.Random.Guid().ToString();
        var expectedTitle = _faker.Name.JobTitle();

        var season = new Season(expectedId, expectedTitle);

        var raceControl = new RaceGrandPix(circuit, season);

        raceControl.StartSession(EType.FreePractice1);

        raceControl.Session.Should().NotBeNull();
        var session = raceControl.Session.FirstOrDefault(s => s.Type == EType.FreePractice1);
        session.Status.Should().Be(EStatus.Live);
    }

    [Theory]
    [InlineData(EType.FreePractice2)]
    [InlineData(EType.FreePractice3)]
    [InlineData(EType.Qualifying)]
    [InlineData(EType.MainRace)]
    public void StartSession_PreviousSessionNotFinished_ThrowsDomainException(EType typeSession)
    {
        var expectedIdCircuit = _faker.Random.Guid().ToString();
        var expectedNameCircuit = _faker.Name.JobArea();
        var expectedCountry = _faker.Address.Country();
        var expectedLaps = _faker.Random.Number(44, 77);

        var circuit = new Circuit(expectedIdCircuit, expectedNameCircuit, expectedCountry, expectedLaps);

        var expectedId = _faker.Random.Guid().ToString();
        var expectedTitle = _faker.Name.JobTitle();

        var season = new Season(expectedId, expectedTitle);

        var raceControl = new RaceGrandPix(circuit, season);

        Assert.Throws<Exception>(() => raceControl.StartSession(typeSession));
    }

    [Fact]
    public void UpdateSession_ValidResults_ChangeSessionToFinished()
    {
        var expectedIdCircuit = _faker.Random.Guid().ToString();
        var expectedNameCircuit = _faker.Name.JobArea();
        var expectedCountry = _faker.Address.Country();
        var expectedLaps = _faker.Random.Number(44, 77);

        var circuit = new Circuit(expectedIdCircuit, expectedNameCircuit, expectedCountry, expectedLaps);

        var expectedId = _faker.Random.Guid().ToString();
        var expectedTitle = _faker.Name.JobTitle();

        var season = new Season(expectedId, expectedTitle);

        var raceControl = new RaceGrandPix(circuit, season);

        var constructors = new List<ConstructorChampionship>();
        var drivers = new List<DriverChampionship>();

        for (int i = 0; i < 10; i++)
        {
            var expectedIdTeam = _faker.Random.Number(1, 11);
            var expectedNameTeam = _faker.Name.JobTitle();

            constructors.Add(new ConstructorChampionship(expectedIdTeam, expectedNameTeam));

            var expectedIdDriver = _faker.Random.Number(1, 99);
            var expectedNameDriver = _faker.Name.FirstName();
            var expectedNumber = _faker.Random.Number(1, 99);

            drivers.Add(new DriverChampionship(expectedIdDriver, expectedNameDriver, expectedNumber, expectedIdTeam, expectedNameTeam));
            var driver = drivers.Last();

            driver.SetGridPosition(_faker.Random.Number(1, 22));
            driver.SetPlacing(_faker.Random.Number(1, 22));
            driver.SetPoints(_faker.Random.Number(1, 25));

            var constructor = constructors.FirstOrDefault(c => c.IdTeam == driver.IdTeam);

            if (constructor is not null)
                constructor.SetTotalPoints(driver.Points);
        }

        var sessionResult = new SessionResult(drivers, constructors);

        raceControl.StartSession(EType.FreePractice1);
        raceControl.UpdateResultsSession(EType.FreePractice1, sessionResult);

        raceControl.StartSession(EType.FreePractice2);
        raceControl.UpdateResultsSession(EType.FreePractice2, sessionResult);

        raceControl.StartSession(EType.FreePractice3);
        raceControl.UpdateResultsSession(EType.FreePractice3, sessionResult);

        raceControl.StartSession(EType.Qualifying);
        raceControl.UpdateResultsSession(EType.Qualifying, sessionResult);

        raceControl.StartSession(EType.MainRace);
        raceControl.UpdateResultsSession(EType.MainRace, sessionResult);

        raceControl.Session.Select(s => s.Type == EType.FreePractice1).Should().NotBeNull();
        raceControl.Session.Select(s => s.Type == EType.FreePractice2).Should().NotBeNull();
        raceControl.Session.Select(s => s.Type == EType.FreePractice3).Should().NotBeNull();
        raceControl.Session.Select(s => s.Type == EType.Qualifying).Should().NotBeNull();
        raceControl.Session.Select(s => s.Type == EType.MainRace).Should().NotBeNull();
    }
}
