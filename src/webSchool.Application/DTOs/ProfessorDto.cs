namespace application.DTOs;

public class ProfessorDto
{
    public int? Id { get; set; } // null al crear
    public string? Name { get; set; }
    public string? LastName { get; set; }
    public string? Document { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Specialty { get; set; }
}
