namespace domain.Entities;

public class Student : Person
{
    public List<Inscription>? Inscriptions { get; set; } = new();
}
