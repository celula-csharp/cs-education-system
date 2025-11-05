using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Extensions;

public interface IJwtService
{
    string GenerateToken(User user);
}

public static class JwtServiceExtension
{
    public static IServiceCollection AddJwtService(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSection = configuration.GetSection("Jwt");
        var key = jwtSection["Key"] ?? throw new Exception("JWT Key no configurada");
        var issuer = jwtSection["Issuer"];
        var audience = jwtSection["Audience"];
        var expiry = int.TryParse(jwtSection["ExpiryMinutes"], out var exp) ? exp : 60;

        services.AddSingleton<IJwtService>(new JwtService(key, issuer, audience, expiry));
        return services;
    }

    private class JwtService : IJwtService
    {
        private readonly string _key;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly int _expiryMinutes;

        public JwtService(string key, string issuer, string audience, int expiryMinutes)
        {
            _key = key;
            _issuer = issuer;
            _audience = audience;
            _expiryMinutes = expiryMinutes;
        }

        public string GenerateToken(User user)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.Name, user.UserName),
                new(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_expiryMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
