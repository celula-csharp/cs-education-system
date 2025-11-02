using System.Reflection;
using domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<Student> Students => Set<Student>();
    public DbSet<Professor> Professors => Set<Professor>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Secction> Secctions => Set<Secction>();
    public DbSet<Inscription> Inscriptions => Set<Inscription>();
    public DbSet<Grades> Grades => Set<Grades>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}
