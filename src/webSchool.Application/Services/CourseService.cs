using domain.Entities;
using domain.Interfaces;
using application.DTOs;

namespace application.Services;

using Interfaces;

public class CourseService : ICourseService
{
    private readonly IRepository<Course> _repo;
    private readonly IRepository<Professor> _repoProf;

    public CourseService(IRepository<Course> repo, IRepository<Professor> repoProf)
    {
        _repo = repo;
        _repoProf = repoProf;
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
        if (string.IsNullOrWhiteSpace(dto.CourseName)) throw new ArgumentException("El nombre del curso es obligatorio.");
        if (dto.ProfessorId <= 0) throw new ArgumentException("Debe asignarse un profesor válido.");

        var prof = await _repoProf.ById(dto.ProfessorId);
        if (prof == null) throw new ArgumentException("El profesor no existe.");

        var all = await _repo.All();
        if (all.Any(c => c.CourseName == dto.CourseName && c.ProfessorId == dto.ProfessorId))
            throw new ArgumentException("El profesor ya tiene un curso con ese nombre.");

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

        var all = await _repo.All();
        if (all.Any(c => c.CourseName == dto.CourseName && c.ProfessorId == dto.ProfessorId && c.Id != dto.Id))
            throw new ArgumentException("El profesor ya tiene otro curso con ese nombre.");

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
        if (course.Secctions != null && course.Secctions.Any()) return false;

        var ok = await _repo.Delete(id);
        if (!ok) return false;
        await _repo.Save();
        return true;
    }

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
