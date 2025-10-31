namespace application.Services;

using application.DTOs;
using application.Interfaces;
using domain.Entities;
using domain.Interfaces;

public class ProfessorService : IProfessorService
{
    private readonly IRepository<Professor> _repo;

    public ProfessorService(IRepository<Professor> repo)
    {
        _repo = repo;
    }

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
        // validación simple e intuitiva
        if (string.IsNullOrWhiteSpace(dto.Name)) throw new ArgumentException("El nombre es obligatorio.");
        if (string.IsNullOrWhiteSpace(dto.LastName)) throw new ArgumentException("El apellido es obligatorio.");

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

        // actualizar campos permitidos
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
        // regla simple: no borrar si tiene cursos asociados (chequeo básico)
        var prof = await _repo.ById(id);
        if (prof == null) return false;
        if (prof.Courses != null && prof.Courses.Count != 0) return false;

        var ok = await _repo.Delete(id);
        if (!ok) return false;
        await _repo.Save();
        return true;
    }

    // mapping
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
