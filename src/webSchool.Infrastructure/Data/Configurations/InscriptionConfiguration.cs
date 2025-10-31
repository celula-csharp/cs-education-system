namespace Infrastructure.Data.Configurations;

using domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class InscriptionConfiguration : IEntityTypeConfiguration<Inscription>
{
    public void Configure(EntityTypeBuilder<Inscription> builder)
    {
        builder.ToTable("Inscriptions");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.Date)
            .HasColumnType("datetime");

        // Relación con estudiante
        builder.HasOne(i => i.Student)
            .WithMany(s => s.Inscriptions)
            .HasForeignKey(i => i.StudentId);

        // Relación con sección
        builder.HasOne(i => i.Secction)
            .WithMany()
            .HasForeignKey(i => i.SecctionId);

        // Relación uno a uno con calificación
        builder.HasOne(i => i.Grade)
            .WithOne(g => g.Inscription)
            .HasForeignKey<Grades>(g => g.InscriptionId);
    }
}
