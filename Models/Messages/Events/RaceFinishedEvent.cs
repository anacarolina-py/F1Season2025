namespace Domain.Competition.Models.Messages.Events
{
    public class RaceFinishedEvent : BaseMessage
    {
        public int Round { get; }
        public string CircuitId { get; }
        public string CompetitionId { get; }

        public bool IsSeasonCompleted { get; }

        public RaceFinishedEvent(int round, string circuitId, string competitionId, bool isSeasonCompleted)
        {
            Round = round;
            CompetitionId = competitionId;
            CircuitId = circuitId;
            IsSeasonCompleted = isSeasonCompleted;
        }
    }
}
