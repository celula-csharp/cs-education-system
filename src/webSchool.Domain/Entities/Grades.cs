namespace domain.Entities;

public class Grades
{
    public int Id { get; set; }
    public double Grade { get; set; }

    public int InscriptionId { get; set; }
    public Inscription? Inscription { get; set; }
}
