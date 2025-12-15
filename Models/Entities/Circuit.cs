namespace Domain.Competition.Models.Entities;

public class Circuit
{

    public Guid Id { get; set; }
    public string NameCircuit { get; set; }
    public string Country { get; private set; }
    public int Laps { get; private set; }

    public Circuit(string nameCircuit, string country, int laps)
    {
        if (string.IsNullOrWhiteSpace(nameCircuit))
        {
            throw new ArgumentException("You cannot register a circuit without a name");
        }
        if (string.IsNullOrWhiteSpace(country))
        {
            throw new ArgumentException("You cannot register a circuit without a country");
        }
        if (laps <= 0)
        {
            throw new ArgumentException("The number of laps must be greater than zero");
        }
        Id = Guid.NewGuid();
        NameCircuit = nameCircuit;
        Country = country;
        Laps = laps;
    }
    
}
