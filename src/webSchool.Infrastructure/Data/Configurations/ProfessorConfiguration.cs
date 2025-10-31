namespace infrastructure.Data.Configurations;

using domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ProfessorConfiguration  : IEntityTypeConfiguration<Professor>
{
    public void Configure(EntityTypeBuilder<Professor> builder)
    {
        builder.ToTable("Professors");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name).HasMaxLength(100);
        builder.Property(p => p.LastName).HasMaxLength(100);
        builder.Property(p => p.Document).HasMaxLength(50);
        builder.Property(p => p.Email).HasMaxLength(120);
        builder.Property(p => p.Phone).HasMaxLength(20);
        builder.Property(p => p.Specialty).HasMaxLength(100);

        // Relación: un profesor puede tener varios cursos
        builder.HasMany<Course>()
            .WithOne(c => c.Professor)
            .HasForeignKey(c => c.ProfessorId);
    }
}
