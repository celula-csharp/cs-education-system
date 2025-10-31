namespace Infrastructure.Data.Configurations;

using domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class SecctionConfiguration : IEntityTypeConfiguration<Secction>
{
    public void Configure(EntityTypeBuilder<Secction> builder)
    {
        builder.ToTable("Secctions");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Day).HasMaxLength(20);
        builder.Property(s => s.Classroom).HasMaxLength(50);

        // Relación con curso
        builder.HasOne(s => s.Course)
            .WithMany(c => c.Secctions)
            .HasForeignKey(s => s.CourseId);

        // Relación con inscripciones
        builder.HasMany<Inscription>()
            .WithOne(i => i.Secction)
            .HasForeignKey(i => i.SecctionId);
    }
}
