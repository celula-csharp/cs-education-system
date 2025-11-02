namespace domain.Entities;

public class Secction
{
    public int Id { get; set; }
    public string? Day { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public string? Classroom { get; set; }
    public int Capacity { get; set; }

    public int CourseId { get; set; }
    public Course? Course { get; set; }

    public List<Inscription>? Inscriptions { get; set; } = new();
}
