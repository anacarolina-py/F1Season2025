using Domain.Competition.Models.Entities;

namespace F1Season2025.Competition.Repository.Interfaces
{
    public interface ICircuitRepository
    {
        Task<Circuit> CreateCircuitAsync(Circuit circuit);
        Task<Circuit?> GetByIdAsync(Guid id);
        Task<List<Circuit>> GetAllAsync();
    }

}
