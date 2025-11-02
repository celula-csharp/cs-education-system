namespace Infrastructure.Data.Configurations;

using domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.ToTable("Courses");
        builder.HasKey(c => c.Id);

        builder.Property(c => c.CourseName).HasMaxLength(100);
        builder.Property(c => c.CourseDescription).HasMaxLength(250);
        builder.Property(c => c.CourseDuration).HasMaxLength(50);

        // índice único: nombre por profesor
        builder.HasIndex(c => new { c.CourseName, c.ProfessorId }).IsUnique();

        builder.HasOne(c => c.Professor)
            .WithMany(p => p.Courses)
            .HasForeignKey(c => c.ProfessorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(c => c.Secctions)
            .WithOne(s => s.Course)
            .HasForeignKey(s => s.CourseId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
