namespace application.DTOs;

public class GradeDto
{
    public int? Id { get; set; }
    public double Grade { get; set; } // entre 0 y 5, o el rango que manejes
    public int InscriptionId { get; set; }
}
