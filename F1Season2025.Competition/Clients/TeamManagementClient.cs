using Domain.Competition.Models.DTOs.Competitions;
using System.Text.Json;

namespace F1Season2025.Competition.Clients
{
    public class TeamManagementClient
    {
        private readonly HttpClient _httpClient;

        public TeamManagementClient(HttpClient httpClientTeams)
        {
            _httpClient = httpClientTeams;
        }

        public async Task<ValidateTeamSeasonDto> ValidateSeasonAsync()
        {
            var response = await _httpClient.GetAsync(""/*endpoint da api teams*/);

            if (!response.IsSuccessStatusCode)
            {
                throw new InvalidOperationException($"TeamManagement error: {response.StatusCode}");
            }

            var json = await response.Content.ReadAsStringAsync();

            var teamValidation = JsonSerializer.Deserialize<ValidateTeamSeasonDto>(json);

            if (teamValidation == null)
            {
                throw new InvalidOperationException(
                    "Invalid response from TeamManagement");
            }

            return teamValidation;
        }
    }
}
