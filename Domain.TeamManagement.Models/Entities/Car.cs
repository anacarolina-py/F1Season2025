namespace Domain.TeamManagement.Models.Entities
{
    public class Car
    {
        public int CarId { get; private set; }

        public string Model { get; private set; }

        public decimal AerodynamicCoefficient { get; private set; }

        public decimal PowerCoefficient { get; private set; }

        public decimal Weight { get; private set; }

        public string Status { get; private set; }

        public int TeamId { get; private set; }

        public int DriverId { get; private set; }

        public int AerodynamicEngineerId { get; private set; }

        public int PowerEngineerId { get; private set; }


    }
}
