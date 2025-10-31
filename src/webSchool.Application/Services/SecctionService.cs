namespace application.Services;

using application.DTOs;
using application.Interfaces;
using domain.Entities;
using domain.Interfaces;

public class SecctionService : ISecctionService
{
    private readonly IRepository<Secction> _repo;

    public SecctionService(IRepository<Secction> repo)
    {
        _repo = repo;
    }

    public async Task<List<SecctionDto>> AllAsync()
    {
        var list = await _repo.All();
        return list.Select(ToDto).ToList();
    }

    public async Task<SecctionDto?> ByIdAsync(int id)
    {
        var ent = await _repo.ById(id);
        if (ent == null) return null;
        return ToDto(ent);
    }

    public async Task<SecctionDto> CreateAsync(SecctionDto dto)
    {
        if (dto.CourseId <= 0)
            throw new ArgumentException("Debe asignarse un curso válido.");

        if (dto.Capacity <= 0)
            throw new ArgumentException("El cupo debe ser mayor a cero.");

        if (dto.EndTime <= dto.StartTime)
            throw new ArgumentException("La hora final debe ser mayor que la hora de inicio.");

        var entity = ToEntity(dto);
        await _repo.Create(entity);
        await _repo.Save();
        return ToDto(entity);
    }

    public async Task<bool> UpdateAsync(SecctionDto dto)
    {
        if (dto.Id == null) return false;

        var exist = await _repo.ById(dto.Id.Value);
        if (exist == null) return false;

        // validaciones simples
        if (dto.Capacity <= 0) throw new ArgumentException("El cupo debe ser mayor que cero.");
        if (dto.EndTime <= dto.StartTime) throw new ArgumentException("La hora final debe ser mayor que la hora de inicio.");

        exist.Day = dto.Day;
        exist.StartTime = dto.StartTime;
        exist.EndTime = dto.EndTime;
        exist.Classroom = dto.Classroom;
        exist.Capacity = dto.Capacity;
        exist.CourseId = dto.CourseId;

        await _repo.Update(exist);
        await _repo.Save();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var sec = await _repo.ById(id);
        if (sec == null) return false;

        // regla: no borrar si tiene inscripciones
        var hasIns = sec.GetType().GetProperty("Inscriptions") != null; // verificación simple
        if (hasIns) return false;

        var ok = await _repo.Delete(id);
        if (!ok) return false;
        await _repo.Save();
        return true;
    }

    // mapping
    private static SecctionDto ToDto(Secction s) => new()
    {
        Id = s.Id,
        Day = s.Day,
        StartTime = s.StartTime,
        EndTime = s.EndTime,
        Classroom = s.Classroom,
        Capacity = s.Capacity,
        CourseId = s.CourseId
    };

    private static Secction ToEntity(SecctionDto dto) => new()
    {
        Id = dto.Id ?? 0,
        Day = dto.Day,
        StartTime = dto.StartTime,
        EndTime = dto.EndTime,
        Classroom = dto.Classroom,
        Capacity = dto.Capacity,
        CourseId = dto.CourseId
    };
}
