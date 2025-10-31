namespace application.Interfaces;

using domain.Entities;
using application.DTOs;

public interface IGradeService : IGenericService<Grades, GradeDto>
{
    // podríamos agregar cálculos o estadísticas más adelante
}
