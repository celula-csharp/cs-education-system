namespace application.Extensions;

using Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionsEx
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Aquí se registrarán los servicios, validadores o mapeadores de la capa Application
        return services;
    }
}
