namespace domain.Entities;

public class Inscription
{
    public int Id { get; set; }
    public DateTime Date { get; set; } =  DateTime.UtcNow;
    public int StudentId { get; set; }
    public Student Student { get; set; } = default!;
    public int SecctionId { get; set; }
    public Secction Secction { get; set; } = default!;
    public Grades Grade { get; set; } = default!;
}
