using domain.Entities;
using domain.Interfaces;
using application.DTOs;

namespace application.Services;

using Interfaces;

public class InscriptionService : IInscriptionService
{
    private readonly IRepository<Inscription> _repo;
    private readonly IRepository<Secction> _repoSecction;
    private readonly IRepository<Student> _repoStudent;

    public InscriptionService(IRepository<Inscription> repo,
                              IRepository<Secction> repoSecction,
                              IRepository<Student> repoStudent)
    {
        _repo = repo;
        _repoSecction = repoSecction;
        _repoStudent = repoStudent;
    }

    public async Task<List<InscriptionDto>> AllAsync()
    {
        var list = await _repo.All();
        return list.Select(ToDto).ToList();
    }

    public async Task<InscriptionDto?> ByIdAsync(int id)
    {
        var ent = await _repo.ById(id);
        if (ent == null) return null;
        return ToDto(ent);
    }

    public async Task<InscriptionDto> CreateAsync(InscriptionDto dto)
    {
        if (dto.StudentId <= 0 || dto.SecctionId <= 0)
            throw new ArgumentException("Estudiante y Sección son requeridos.");

        var sec = await _repoSecction.ById(dto.SecctionId);
        if (sec == null) throw new ArgumentException("La sección no existe.");

        var student = await _repoStudent.ById(dto.StudentId);
        if (student == null) throw new ArgumentException("El estudiante no existe.");

        // traer inscripciones y secciones para validar
        var inscripciones = await _repo.All();
        var secctions = await _repoSecction.All();

        // duplicado exacto
        if (inscripciones.Any(i => i.StudentId == dto.StudentId && i.SecctionId == dto.SecctionId))
            throw new ArgumentException("El estudiante ya está inscrito en esa sección.");

        // cupo
        int ocupados = inscripciones.Count(i => i.SecctionId == dto.SecctionId);
        if (ocupados >= sec.Capacity) throw new ArgumentException("La sección ya alcanzó el cupo máximo.");

        // conflicto horario: comparar sección destino con secciones inscritas del estudiante
        var inscripcionesEst = inscripciones.Where(i => i.StudentId == dto.StudentId).ToList();

        bool conflicto = inscripcionesEst.Any(i =>
        {
            var s = secctions.FirstOrDefault(x => x.Id == i.SecctionId);
            if (s == null) return false;
            return s.Day == sec.Day && s.StartTime < sec.EndTime && s.EndTime > sec.StartTime;
        });

        if (conflicto) throw new ArgumentException("El estudiante ya tiene una clase en ese horario.");

        var entity = ToEntity(dto);
        await _repo.Create(entity);
        await _repo.Save();
        return ToDto(entity);
    }

    public async Task<bool> UpdateAsync(InscriptionDto dto)
    {
        if (dto.Id == null) return false;
        var exist = await _repo.ById(dto.Id.Value);
        if (exist == null) return false;

        exist.StudentId = dto.StudentId;
        exist.SecctionId = dto.SecctionId;
        exist.Date = dto.Date;

        await _repo.Update(exist);
        await _repo.Save();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var ins = await _repo.ById(id);
        if (ins == null) return false;

        var ok = await _repo.Delete(id);
        if (!ok) return false;
        await _repo.Save();
        return true;
    }

    private static InscriptionDto ToDto(Inscription i) => new()
    {
        Id = i.Id,
        StudentId = i.StudentId,
        SecctionId = i.SecctionId,
        Date = i.Date,
        Grade = i.Grade?.Grade
    };

    private static Inscription ToEntity(InscriptionDto dto) => new()
    {
        Id = dto.Id ?? 0,
        StudentId = dto.StudentId,
        SecctionId = dto.SecctionId,
        Date = dto.Date
    };
}
