namespace F1Season2025.Competition.Services.Interfaces
{
    public interface ITeamServiceClient
    {
        Task<bool> ValidateSeasonAsync();
    }
}
