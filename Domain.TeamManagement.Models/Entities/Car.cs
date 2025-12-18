namespace Domain.TeamManagement.Models.Entities;

public class Car
{
    public int CarId { get; private set; }

    public string Model { get; private set; }

    public decimal AerodynamicCoefficient { get; private set; }

    public decimal PowerCoefficient { get; private set; }

    public decimal Weight { get; private set; }

    public string Status { get; private set; }

    public Car(string model, decimal weight)
    {
        Model = model;
        AerodynamicCoefficient = (decimal)(new Random().Next(10000)) / (decimal)1000.00;
        PowerCoefficient = (decimal)(new Random().Next(10000)) / (decimal)1000.00;
        Weight = weight;
        Status = "Inativo";
    }
}
