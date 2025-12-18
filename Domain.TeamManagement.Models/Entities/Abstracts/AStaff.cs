namespace Domain.TeamManagement.Models.Entities.Abstracts;

public abstract class AStaff
{
    public int StaffId { get; private set; }

    public string FirstName { get; private set; }

    public string LastName { get; private set; }

    public int Age { get; private set; }

    public decimal Experience { get; private set; }

    public string Status { get; private set; }

    public AStaff(string firstName, string lastName, int age)
    {
        FirstName = firstName;
        LastName = lastName;
        Age = age;
        Experience = (decimal)( new Random().Next(1000,5000)) / (decimal)(1000);
        Status = "Inativo";
    }
}
