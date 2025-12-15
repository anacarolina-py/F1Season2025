using Domain.Competition.Models.Entities.Enum;

namespace Domain.Competition.Models.Entities
{
    public class Competition
    {
        public Guid Id { get; private set; }
        public int Round { get; private set; }
        public Guid CircuitId { get; private set; }
        public Circuit Circuit { get; private set; }
        public CompetitionStatus Status { get; private set; }
        public bool IsCompleted { get; private set; }

        public Competition() { }

        public Competition(int round, Circuit circuit, bool isCompleted)
        {
            Id = Guid.NewGuid();
            Round = round;
            Circuit = circuit ?? 
                 throw new ArgumentNullException(nameof(circuit));
            Status = CompetitionStatus.Scheduled;
            IsCompleted = isCompleted;
        }

        public (bool IsValid, string Error) ValidateCircuitRace(Competition? competitionNext)
        {
            if (!IsCompleted)
            {
                return (false, "The race is inactive");
            }
            if (Status != CompetitionStatus.Scheduled)
            {
                return (false, $"Round: {Round} is already {Status}");
            }
            if (Round == 1)
            {
                return (true, "Season opener is elegible");
            }
            if (competitionNext is null)
            {
                return (false, "Next round missing");
            }
            if (competitionNext.Status != CompetitionStatus.Finished)
            {
                return (false, $"Next round: {competitionNext.Round} is not finished");
            }
            return (true, "Circuit race is elegible");
        }
        public void StartCompetition()
        {
            if (!IsCompleted)
            {
                throw new InvalidOperationException("Cannot complete an inactive competition");
            }
            if (Status != CompetitionStatus.Scheduled)
            {
                throw new InvalidOperationException($"Cannot start race, race status: {Status}");
            }
            Status = CompetitionStatus.InProgress;
        }
        public void CompleteCompetition()
        {
            if (!IsCompleted)
            {
                throw new InvalidOperationException("Cannot complete an inactive competition");
            }
            if (Status != CompetitionStatus.InProgress)
            {
                throw new InvalidOperationException($"Cannot finish race It must be in progress first. race status: {Status}");
            }
            Status = CompetitionStatus.Finished;
        }
    }
}
