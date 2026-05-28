using DataAccessLayer.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Configurations;

public class SubjectClassConfiguration : IEntityTypeConfiguration<SubjectClass>
{
    public void Configure(EntityTypeBuilder<SubjectClass> builder)
    {
        builder.ToTable("subject_classes");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.SubjectCode)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(e => e.SubjectName)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(e => e.ClassName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.Semester)
            .IsRequired()
            .HasMaxLength(50);

    }
}
