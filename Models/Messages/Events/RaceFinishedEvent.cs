namespace Domain.Competition.Models.Messages.Events
{
    public class RaceFinishedEvent
    {
        public int Round { get; }
        public Guid CircuitId { get; }
        public bool IsSeasonCompleted { get; }

        public RaceFinishedEvent(int round, Guid circuitId, bool isSeasonCompleted)
        {
            Round = round;
            CircuitId = circuitId;
            IsSeasonCompleted = isSeasonCompleted;
        }
    }
}
