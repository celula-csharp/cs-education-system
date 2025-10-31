namespace application.Services;

using application.DTOs;
using application.Interfaces;
using domain.Entities;
using domain.Interfaces;

public class GradeService : IGradeService
{
    private readonly IRepository<Grades> _repo;
    private readonly IRepository<Inscription> _repoIns;

    public GradeService(IRepository<Grades> repo, IRepository<Inscription> repoIns)
    {
        _repo = repo;
        _repoIns = repoIns;
    }

    public async Task<List<GradeDto>> AllAsync()
    {
        var list = await _repo.All();
        return list.Select(ToDto).ToList();
    }

    public async Task<GradeDto?> ByIdAsync(int id)
    {
        var g = await _repo.ById(id);
        if (g == null) return null;
        return ToDto(g);
    }

    public async Task<GradeDto> CreateAsync(GradeDto dto)
    {
        if (dto.Grade < 0 || dto.Grade > 5)
            throw new ArgumentException("La nota debe estar entre 0 y 5.");

        if (dto.InscriptionId <= 0)
            throw new ArgumentException("Debe asignarse una inscripción válida.");

        var ins = await _repoIns.ById(dto.InscriptionId);
        if (ins == null)
            throw new ArgumentException("La inscripción no existe.");

        // Evitar duplicar calificación
        var all = await _repo.All();
        if (all.Any(g => g.InscriptionId == dto.InscriptionId))
            throw new ArgumentException("La inscripción ya tiene una nota asignada.");

        var entity = ToEntity(dto);
        await _repo.Create(entity);
        await _repo.Save();
        return ToDto(entity);
    }

    public async Task<bool> UpdateAsync(GradeDto dto)
    {
        if (dto.Id == null) return false;
        if (dto.Grade < 0 || dto.Grade > 5)
            throw new ArgumentException("La nota debe estar entre 0 y 5.");

        var exist = await _repo.ById(dto.Id.Value);
        if (exist == null) return false;

        exist.Grade = dto.Grade;
        exist.InscriptionId = dto.InscriptionId;

        await _repo.Update(exist);
        await _repo.Save();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var g = await _repo.ById(id);
        if (g == null) return false;

        var ok = await _repo.Delete(id);
        if (!ok) return false;
        await _repo.Save();
        return true;
    }

    // mappers
    private static GradeDto ToDto(Grades g) => new()
    {
        Id = g.Id,
        Grade = g.Grade,
        InscriptionId = g.InscriptionId
    };

    private static Grades ToEntity(GradeDto dto) => new()
    {
        Id = dto.Id ?? 0,
        Grade = dto.Grade,
        InscriptionId = dto.InscriptionId
    };
}
