namespace domain.Entities;

public class Professor : Person
{
    public string? Specialty { get; set; }
    public List<Course>? Courses { get; set; } = new();
}
