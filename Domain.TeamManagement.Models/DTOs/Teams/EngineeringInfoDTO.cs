using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.TeamManagement.Models.DTOs.Teams
{
    public class EngineeringInfoDTO
    {
        public int TeamId { get; set; }
        public int CarId { get; set; }
        public int DriverId { get; set; }
        public decimal AerodynamicCoefficient { get; set; }
        public decimal PowerCoefficient { get; set; }
        public decimal? EngineerExperienceCa { get; set; }
        public decimal? EngineerExperienceCp { get; set; }
        public decimal DriverHandicap { get; set; }
        public decimal DriverExperience { get; set; }
        public decimal? Pd { get; set; }


    }
}
