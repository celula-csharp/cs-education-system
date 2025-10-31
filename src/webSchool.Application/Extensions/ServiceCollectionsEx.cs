namespace application.Extensions;

using Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Services;

public static class ServiceCollectionsEx
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Aquí se registrarán los servicios, validadores o mapeadores de la capa Application

        services.AddScoped<IProfessorService, ProfessorService>();
        services.AddScoped<IStudentService, StudentService>();
        services.AddScoped<ICourseService, CourseService>();
        services.AddScoped<ISecctionService, SecctionService>();
        services.AddScoped<IInscriptionService, InscriptionService>();
        services.AddScoped<IGradeService, GradeService>();
        return services;
    }
}
