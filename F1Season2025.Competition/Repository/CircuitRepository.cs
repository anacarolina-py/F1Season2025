using Domain.Competition.Models.Entities;
using F1Season2025.Competition.Data;
using F1Season2025.Competition.Repository.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace F1Season2025.Competition.Repository
{
    public class CircuitRepository : ICircuitRepository
    {
        private readonly ILogger<CircuitRepository> _logger;
        private readonly IMongoCollection<Circuit> _collection;

        public CircuitRepository(ILogger<CircuitRepository> logger, ConnectionDB connection)
        {
            _logger = logger;
            _collection = connection.GetMongoCollection<Circuit>("circuits"); ;
        }

        public async Task AddCircuitAsync(Circuit circuit) 
        {
            _logger.LogInformation("Adding circuit {CircuitName} to database", circuit.NameCircuit);

            await _collection.InsertOneAsync(circuit);
        }

        public async Task<Circuit?> GetCircuitByIdAsync(ObjectId id) 
        {
            _logger.LogInformation("Searching circuit with Id {CircuitId}", id);

            return await _collection
                .Find(c => c.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Circuit>> GetAllCircuitsAsync() 
        {
            _logger.LogInformation("Listing all circuits");

            return await _collection
               .Find(_ => true)
               .ToListAsync();
        }
    }
}
