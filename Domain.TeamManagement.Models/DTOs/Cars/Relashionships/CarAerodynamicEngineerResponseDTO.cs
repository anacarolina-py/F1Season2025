using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.TeamManagement.Models.DTOs.Cars.Relashionships
{
    public class CarAerodynamicEngineerResponseDTO
    {
        public int AerodynamicEngineerId { get; init; }

        public int CarId { get; init; }

        public string Status { get; init; }
    }
}
