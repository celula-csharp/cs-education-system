namespace application.Interfaces;

using domain.Entities;
using application.DTOs;

public interface ICourseService : IGenericService<Course, CourseDto>
{
    // futuros métodos específicos del curso
}
