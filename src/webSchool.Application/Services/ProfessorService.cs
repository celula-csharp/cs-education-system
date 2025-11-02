using System.Text.RegularExpressions;
using domain.Entities;
using domain.Interfaces;
using application.DTOs;

namespace application.Services;

using Interfaces;

public class ProfessorService : IProfessorService
{
    private readonly IRepository<Professor> _repo;

    public ProfessorService(IRepository<Professor> repo) => _repo = repo;

    public async Task<List<ProfessorDto>> AllAsync()
    {
        var list = await _repo.All();
        return list.Select(ToDto).ToList();
    }

    public async Task<ProfessorDto?> ByIdAsync(int id)
    {
        var e = await _repo.ById(id);
        if (e == null) return null;
        return ToDto(e);
    }

    public async Task<ProfessorDto> CreateAsync(ProfessorDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name)) throw new ArgumentException("El nombre es obligatorio.");
        if (string.IsNullOrWhiteSpace(dto.LastName)) throw new ArgumentException("El apellido es obligatorio.");
        if (string.IsNullOrWhiteSpace(dto.Specialty)) throw new ArgumentException("La especialidad es obligatoria.");

        if (!string.IsNullOrWhiteSpace(dto.Email) && !Regex.IsMatch(dto.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            throw new ArgumentException("Correo no válido.");

        var all = await _repo.All();
        if (!string.IsNullOrWhiteSpace(dto.Document) && all.Any(p => p.Document == dto.Document))
            throw new ArgumentException("Documento ya registrado.");
        if (!string.IsNullOrWhiteSpace(dto.Email) && all.Any(p => p.Email == dto.Email))
            throw new ArgumentException("Correo ya registrado.");

        var entity = ToEntity(dto);
        await _repo.Create(entity);
        await _repo.Save();
        return ToDto(entity);
    }

    public async Task<bool> UpdateAsync(ProfessorDto dto)
    {
        if (dto.Id == null) return false;
        var exist = await _repo.ById(dto.Id.Value);
        if (exist == null) return false;

        var all = await _repo.All();
        if (!string.IsNullOrWhiteSpace(dto.Document) && all.Any(p => p.Document == dto.Document && p.Id != dto.Id))
            throw new ArgumentException("Documento ya en uso.");
        if (!string.IsNullOrWhiteSpace(dto.Email) && all.Any(p => p.Email == dto.Email && p.Id != dto.Id))
            throw new ArgumentException("Correo ya en uso.");

        exist.Name = dto.Name;
        exist.LastName = dto.LastName;
        exist.Email = dto.Email;
        exist.Phone = dto.Phone;
        exist.Document = dto.Document;
        exist.Specialty = dto.Specialty;

        await _repo.Update(exist);
        await _repo.Save();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var prof = await _repo.ById(id);
        if (prof == null) return false;
        if (prof.Courses != null && prof.Courses.Any()) return false;

        var ok = await _repo.Delete(id);
        if (!ok) return false;
        await _repo.Save();
        return true;
    }

    private static ProfessorDto ToDto(Professor p) => new()
    {
        Id = p.Id,
        Name = p.Name,
        LastName = p.LastName,
        Document = p.Document,
        Email = p.Email,
        Phone = p.Phone,
        Specialty = p.Specialty
    };

    private static Professor ToEntity(ProfessorDto dto) => new()
    {
        Id = dto.Id ?? 0,
        Name = dto.Name,
        LastName = dto.LastName,
        Document = dto.Document,
        Email = dto.Email,
        Phone = dto.Phone,
        Specialty = dto.Specialty
    };
}
