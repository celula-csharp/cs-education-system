namespace Infrastructure.Data.Configurations;

using domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class GradesConfiguration : IEntityTypeConfiguration<Grades>
{
    public void Configure(EntityTypeBuilder<Grades> builder)
    {
        builder.ToTable("Grades");

        builder.HasKey(g => g.Id);

        builder.Property(g => g.Grade)
            .HasPrecision(5, 2);

        // Relación con inscripción
        builder.HasOne(g => g.Inscription)
            .WithOne(i => i.Grade)
            .HasForeignKey<Grades>(g => g.InscriptionId);
    }
}
