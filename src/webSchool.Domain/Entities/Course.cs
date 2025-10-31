namespace domain.Entities;

public class Course
{
    public int Id { get; set; }
    public string? CourseName { get; set; }
    public string? CourseDescription { get; set; }
    public string? CourseDuration { get; set; }

    public int ProfessorId { get; set; }
    public Professor? Professor { get; set; }

    public List<Secction>? Secctions { get; set; } = new();
}
