namespace application.DTOs;

public class InscriptionDto
{
    public int? Id { get; set; }
    public DateTime Date { get; set; } = DateTime.UtcNow;

    public int StudentId { get; set; }
    public int SecctionId { get; set; }

    public double? Grade { get; set; }
}
