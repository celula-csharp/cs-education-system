using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions;

using domain.Interfaces;
using infrastructure.Data;
using infrastructure.Repositories;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<AppDbContext>(options =>
            options.UseMySql(
                connectionString,
                new MySqlServerVersion(new Version(8, 0, 36))
            )
        );

        services.AddJwtService(configuration);
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        return services;
    }
}
