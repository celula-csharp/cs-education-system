namespace application.DTOs;

public class CourseDto
{
    public int? Id { get; set; }
    public string? CourseName { get; set; }
    public string? CourseDescription { get; set; }
    public string? CourseDuration { get; set; }

    public int ProfessorId { get; set; } // requerido
}
