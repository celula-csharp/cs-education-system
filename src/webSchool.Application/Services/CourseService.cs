namespace application.Services;

using application.DTOs;
using application.Interfaces;
using domain.Entities;
using domain.Interfaces;

public class CourseService : ICourseService
{
    private readonly IRepository<Course> _repo;

    public CourseService(IRepository<Course> repo)
    {
        _repo = repo;
    }

    public async Task<List<CourseDto>> AllAsync()
    {
        var list = await _repo.All();
        return list.Select(ToDto).ToList();
    }

    public async Task<CourseDto?> ByIdAsync(int id)
    {
        var entity = await _repo.ById(id);
        if (entity == null) return null;
        return ToDto(entity);
    }

    public async Task<CourseDto> CreateAsync(CourseDto dto)
    {
        // validaciones básicas
        if (string.IsNullOrWhiteSpace(dto.CourseName))
            throw new ArgumentException("El nombre del curso es obligatorio.");

        if (dto.ProfessorId <= 0)
            throw new ArgumentException("Debe asignarse un profesor válido.");

        var entity = ToEntity(dto);
        await _repo.Create(entity);
        await _repo.Save();
        return ToDto(entity);
    }

    public async Task<bool> UpdateAsync(CourseDto dto)
    {
        if (dto.Id == null) return false;
        var exist = await _repo.ById(dto.Id.Value);
        if (exist == null) return false;

        exist.CourseName = dto.CourseName;
        exist.CourseDescription = dto.CourseDescription;
        exist.CourseDuration = dto.CourseDuration;
        exist.ProfessorId = dto.ProfessorId;

        await _repo.Update(exist);
        await _repo.Save();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var course = await _repo.ById(id);
        if (course == null) return false;

        // regla: no borrar si tiene secciones asociadas
        if (course.Secctions != null && course.Secctions.Count != 0) return false;

        var ok = await _repo.Delete(id);
        if (!ok) return false;
        await _repo.Save();
        return true;
    }

    // mapping
    private static CourseDto ToDto(Course c) => new()
    {
        Id = c.Id,
        CourseName = c.CourseName,
        CourseDescription = c.CourseDescription,
        CourseDuration = c.CourseDuration,
        ProfessorId = c.ProfessorId
    };

    private static Course ToEntity(CourseDto dto) => new()
    {
        Id = dto.Id ?? 0,
        CourseName = dto.CourseName,
        CourseDescription = dto.CourseDescription,
        CourseDuration = dto.CourseDuration,
        ProfessorId = dto.ProfessorId
    };
}
