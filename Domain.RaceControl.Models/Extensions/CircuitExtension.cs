using Domain.RaceControl.Models.DTOs;
using Domain.RaceControl.Models.Entities;

namespace Domain.RaceControl.Models.Extensions;

public static class CircuitExtension
{
    public static Circuit ToEntity(this CircuitResponseDto circuitResponseDto)
    {
        if (circuitResponseDto is null)
            throw new ArgumentNullException("Circuit can't be null");

        return new Circuit(
            circuitResponseDto.IdCircuit,
            circuitResponseDto.NameCircuit,
            circuitResponseDto.Country,
            circuitResponseDto.Laps
            );
    }

    public static CircuitResponseDto ToDto(this Circuit circuit)
    {
        return new CircuitResponseDto
        {
            IdCircuit = circuit.IdCircuit,
            Country = circuit.Country,
            NameCircuit = circuit.NameCircuit,
            Laps = circuit.Laps
        };
    }
}
