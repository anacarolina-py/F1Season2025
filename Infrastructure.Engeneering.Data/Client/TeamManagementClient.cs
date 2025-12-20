using Domain.Engeneering.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;

namespace Infrastructure.Engeneering.Data.Client
{
    public class TeamManagementClient
    {
        private readonly HttpClient _client;

        public TeamManagementClient(HttpClient client)
        {
            _client = client;
        }

        public async Task<EngineeringInfoDTO?> GetEngineeringInfo(int teamId)
        {
            return await _client.GetFromJsonAsync<EngineeringInfoDTO>
                ($"teams/{teamId}/engineeringinfo");
        }
    }
}
