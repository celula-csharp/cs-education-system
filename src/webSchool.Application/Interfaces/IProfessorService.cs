namespace application.Interfaces;

using domain.Entities;
using application.DTOs;

public interface IProfessorService : IGenericService<Professor, ProfessorDto>
{
    // métodos adicionales relacionados al profesor (si aparecen)
}
