using CompetitionEntity = Domain.Competition.Models.Entities.Competition;
using F1Season2025.Competition.Data;
using F1Season2025.Competition.Repository.Interfaces;
using MongoDB.Driver;

namespace F1Season2025.Competition.Repository
{
    public class CompetitionRepository : ICompetitionRepository
    {
        /*private readonly ILogger<CompetitionRepository> _logger;

        private readonly IMongoCollection<CompetitionEntity> _collection;

        private readonly ConnectionDB _connection;

        public CompetitionRepository(ILogger<CompetitionRepository> logger, ConnectionDB connection)
        {
            _logger = logger;
            _connection = connection;
            _collection = _connection.GetMongoCollection();
        }

        public async Task<CompetitionEntity> CreateCompetitionAsync(CompetitionEntity competition)
        {
            _logger.LogInformation("Creating competition for round {Round}", competition.Round);

            await _collection.InsertOneAsync(competition);
            return competition;
        }

        public async Task<List<CompetitionEntity>> GetAllCircuitAsync()
        {
            _logger.LogInformation("Listing all circuits");

            return await _collection
                 .Find(_ => true)
                 .ToListAsync();
        }

        public async Task<CompetitionEntity?> GetByRoundAsync(int round) 
        {
            _logger.LogInformation("Searching competition for round {Round}", round);

            return await _collection
                .Find(c => c.Round == round)
                .FirstOrDefaultAsync();
        }

        public async Task<List<CompetitionEntity>> GetAllCompetitionAsync() 
        {
            _logger.LogInformation("Listing all competitions");

            return await _collection
                .Find(_ => true)
                .ToListAsync();
        }*/
    }
}
