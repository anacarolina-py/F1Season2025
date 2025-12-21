using Domain.Competition.Models.DTOs.Competitions;
using Domain.Competition.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Competition.Models.Extensions
{
    public static class CircuitExtension
    {
        public static CircuitResponseDto ToCircuitResponseDto(this Circuit circuit)
        {
            return new CircuitResponseDto
            {
                Id = circuit.Id.ToString(),
                CircuitName = circuit.NameCircuit,
                Country = circuit.Country,
                Laps = circuit.Laps,
                IsActive = circuit.IsActive
            };
        }
    }
}
