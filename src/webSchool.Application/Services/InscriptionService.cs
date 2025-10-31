namespace application.Services;

using application.DTOs;
using application.Interfaces;
using domain.Entities;
using domain.Interfaces;
using Microsoft.EntityFrameworkCore;

public class InscriptionService : IInscriptionService
{
    private readonly IRepository<Inscription> _repo;
    private readonly IRepository<Secction> _repoSecction;

    public InscriptionService(IRepository<Inscription> repo, IRepository<Secction> repoSecction)
    {
        _repo = repo;
        _repoSecction = repoSecction;
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
        // Validaciones base
        if (dto.StudentId <= 0 || dto.SecctionId <= 0)
            throw new ArgumentException("Estudiante y Sección son requeridos.");

        // 1️⃣ Validar cupo
        var sec = await _repoSecction.ById(dto.SecctionId);
        if (sec == null)
            throw new ArgumentException("La sección no existe.");

        // Contar inscripciones actuales (simulación sin EF Include)
        var inscripciones = await _repo.All();
        int ocupados = inscripciones.Count(i => i.SecctionId == dto.SecctionId);
        if (ocupados >= sec.Capacity)
            throw new ArgumentException("La sección ya alcanzó el cupo máximo.");

        // 2️⃣ Validar conflicto de horario
        var inscripcionesEst = inscripciones.Where(i => i.StudentId == dto.StudentId).ToList();
        bool conflicto = inscripcionesEst.Any(i =>
            i.Secction != null &&
            i.Secction.Day == sec.Day &&
            i.Secction.StartTime < sec.EndTime &&
            i.Secction.EndTime > sec.StartTime);

        if (conflicto)
            throw new ArgumentException("El estudiante ya tiene una clase en ese horario.");

        // 3️⃣ Crear inscripción
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

        // Si tiene calificación asociada, también eliminarla
        if (ins.Grade != null)
        {
            ins.Grade = null;
        }

        var ok = await _repo.Delete(id);
        if (!ok) return false;
        await _repo.Save();
        return true;
    }

    // mapping
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
