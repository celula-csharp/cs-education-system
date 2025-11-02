namespace Infrastructure.Data.Configurations;

using domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.ToTable("Students");
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Name).HasMaxLength(100);
        builder.Property(s => s.LastName).HasMaxLength(100);
        builder.Property(s => s.Document).HasMaxLength(50);
        builder.Property(s => s.Email).HasMaxLength(120);
        builder.Property(s => s.Phone).HasMaxLength(20);

        // índices únicos
        builder.HasIndex(s => s.Document).IsUnique();
        builder.HasIndex(s => s.Email).IsUnique();

        builder.HasMany(s => s.Inscriptions)
            .WithOne(i => i.Student)
            .HasForeignKey(i => i.StudentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
