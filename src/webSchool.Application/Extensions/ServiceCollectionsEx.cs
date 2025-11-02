using Microsoft.Extensions.DependencyInjection;
using application.Interfaces;
using application.Services;

namespace application.Extensions;

public static class ServiceCollectionsEx
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IStudentService, StudentService>();
        services.AddScoped<IProfessorService, ProfessorService>();
        services.AddScoped<ICourseService, CourseService>();
        services.AddScoped<ISecctionService, SecctionService>();
        services.AddScoped<IInscriptionService, InscriptionService>();
        services.AddScoped<IGradeService, GradeService>();

        return services;
    }
}
