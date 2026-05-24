using DataAccessLayer.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Configurations;

public class ImportedGradeRecordConfiguration : IEntityTypeConfiguration<ImportedGradeRecord>
{
    public void Configure(EntityTypeBuilder<ImportedGradeRecord> builder)
    {
        builder.ToTable("ImportedGradeRecords");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.StudentCode)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.StudentName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.ClassName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.SubjectName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.ProcessScore)
            .HasPrecision(5, 2);

        builder.Property(x => x.MidtermScore)
            .HasPrecision(5, 2);

        builder.Property(x => x.FinalScore)
            .HasPrecision(5, 2);

        builder.Property(x => x.TotalScore)
            .HasPrecision(5, 2);

        builder.Property(x => x.Result)
            .HasMaxLength(50);

        builder.Property(x => x.ImportedAt)
            .IsRequired();

        builder.HasOne(x => x.ImportHistory)
            .WithMany(x => x.GradeRecords)
            .HasForeignKey(x => x.ImportHistoryId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(x => x.StudentCode);
        builder.HasIndex(x => x.ImportedAt);
    }
}
