using application.DTOs;
using application.Interfaces;
using domain.Entities;
using domain.Interfaces;
using Infrastructure.Extensions;

namespace application.Services;

public class AuthService : IAuthService
{
    private readonly IRepository<User> _userRepo;
    private readonly IJwtService _jwt;

    public AuthService(IRepository<User> userRepo, IJwtService jwt)
    {
        _userRepo = userRepo;
        _jwt = jwt;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Email))
            throw new ArgumentException("Email requerido.");

        if (string.IsNullOrWhiteSpace(dto.Password))
            throw new ArgumentException("Password requerido.");

        var users = await _userRepo.All();
        if (users.Any(u => u.Email == dto.Email))
            throw new ArgumentException("El email ya está registrado.");

        var user = new User
        {
            UserName = dto.UserName,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = dto.Role ?? "User"
        };

        await _userRepo.Create(user);
        await _userRepo.Save();

        var token = _jwt.GenerateToken(user);
        return new AuthResponseDto { Token = token, ExpiresAt = DateTime.UtcNow.AddMinutes(60) };
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
    {
        var users = await _userRepo.All();
        var user = users.FirstOrDefault(u => u.Email == dto.Email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            throw new ArgumentException("Credenciales inválidas.");

        var token = _jwt.GenerateToken(user);
        return new AuthResponseDto { Token = token, ExpiresAt = DateTime.UtcNow.AddMinutes(60) };
    }
}
