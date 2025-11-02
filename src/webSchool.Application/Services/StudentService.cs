using System.Text.RegularExpressions;
using domain.Entities;
using domain.Interfaces;
using application.DTOs;

namespace application.Services;

using Interfaces;

public class StudentService : IStudentService
{
    private readonly IRepository<Student> _repo;

    public StudentService(IRepository<Student> repo) => _repo = repo;

    public async Task<List<StudentDto>> AllAsync()
    {
        var list = await _repo.All();
        return list.Select(ToDto).ToList();
    }

    public async Task<StudentDto?> ByIdAsync(int id)
    {
        var search = await _repo.ById(id);
        if (search == null) return null;
        return ToDto(search);
    }

    public async Task<StudentDto> CreateAsync(StudentDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name)) throw new ArgumentException("El nombre es obligatorio.");
        if (string.IsNullOrWhiteSpace(dto.Document)) throw new ArgumentException("El documento es obligatorio.");

        // Email formato (si provisto)
        if (!string.IsNullOrWhiteSpace(dto.Email) && !Regex.IsMatch(dto.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            throw new ArgumentException("Correo no válido.");

        var all = await _repo.All();
        if (all.Any(s => s.Document == dto.Document))
            throw new ArgumentException("Documento ya registrado.");
        if (!string.IsNullOrWhiteSpace(dto.Email) && all.Any(s => s.Email == dto.Email))
            throw new ArgumentException("Correo ya registrado.");

        var entity = ToEntity(dto);
        await _repo.Create(entity);
        await _repo.Save();
        return ToDto(entity);
    }

    public async Task<bool> UpdateAsync(StudentDto dto)
    {
        if (dto.Id == null) return false;
        var exist = await _repo.ById(dto.Id.Value);
        if (exist == null) return false;

        // verificar cambios de documento/email a duplicados
        var all = await _repo.All();
        if (!string.IsNullOrWhiteSpace(dto.Document) && all.Any(s => s.Document == dto.Document && s.Id != dto.Id))
            throw new ArgumentException("Documento ya en uso por otro estudiante.");
        if (!string.IsNullOrWhiteSpace(dto.Email) && all.Any(s => s.Email == dto.Email && s.Id != dto.Id))
            throw new ArgumentException("Correo ya en uso por otro estudiante.");

        exist.Name = dto.Name;
        exist.LastName = dto.LastName;
        exist.Email = dto.Email;
        exist.Phone = dto.Phone;
        exist.Document = dto.Document;

        await _repo.Update(exist);
        await _repo.Save();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var ok = await _repo.Delete(id);
        if (!ok) return false;
        await _repo.Save();
        return true;
    }

    private static StudentDto ToDto(Student s) => new()
    {
        Id = s.Id,
        Name = s.Name,
        LastName = s.LastName,
        Email = s.Email,
        Phone = s.Phone,
        Document = s.Document
    };

    private static Student ToEntity(StudentDto dto) => new()
    {
        Id = dto.Id ?? 0,
        Name = dto.Name,
        LastName = dto.LastName,
        Email = dto.Email,
        Phone = dto.Phone,
        Document = dto.Document
    };
}
