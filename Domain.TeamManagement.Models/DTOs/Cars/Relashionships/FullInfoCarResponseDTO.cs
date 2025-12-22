using Domain.TeamManagement.Models.DTOs.Staffs.Drivers;
using Domain.TeamManagement.Models.DTOs.Staffs.Engineers.AerodynamicEngineers;
using Domain.TeamManagement.Models.DTOs.Staffs.Engineers.PowerEngineers;

namespace Domain.TeamManagement.Models.DTOs.Cars.Relashionships;

public class FullInfoCarResponseDTO
{
    public int CarId { get; init; }

    public string Model { get; init; }

    public decimal AerodynamicCoefficient { get; init; }

    public decimal PowerCoefficient { get; init; }

    public decimal Weight { get; init; }

    public List<FullInfoDriverResponseDTO> Drivers { get; init; }

    public List<FullInfoPowerEngineerDTO> PowerEngineers { get; init; }

    public List<FullInfoAerodynamicEngineersDTO> AerodynamicEngineers { get; init; }

}
