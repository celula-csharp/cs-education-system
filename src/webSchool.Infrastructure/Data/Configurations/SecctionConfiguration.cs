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

        builder.HasOne(s => s.Course)
            .WithMany(c => c.Secctions)
            .HasForeignKey(s => s.CourseId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(s => s.Inscriptions)
            .WithOne(i => i.Secction)
            .HasForeignKey(i => i.SecctionId)
            .OnDelete(DeleteBehavior.Restrict);

        // Índice para búsqueda por curso+day+horario (opcional)
        builder.HasIndex(s => new { s.CourseId, s.Day, s.StartTime, s.EndTime });
    }
}
