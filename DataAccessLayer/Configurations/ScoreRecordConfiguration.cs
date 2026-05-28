using DataAccessLayer.Entity;
using DataAccessLayer.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Configurations;

public class ScoreRecordConfiguration : IEntityTypeConfiguration<ScoreRecord>
{
    public void Configure(EntityTypeBuilder<ScoreRecord> builder)
    {
        builder.ToTable("score_records", t =>
        {
            t.HasCheckConstraint(
                "CK_score_records_status_allowed",
                "\"Status\" IN ('Fail', 'Pass')");
        });

        builder.HasKey(e => e.Id);

        builder.Property(e => e.SubjectClassId)
            .IsRequired();

        builder.Property(e => e.RollNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(e => e.FullName)
            .HasMaxLength(255);

        builder.Property(e => e.FinalGrade)
            .HasColumnType("double precision")
            .HasDefaultValue(0);

        builder.Property(e => e.Status)
            .HasConversion<string>()
            .HasMaxLength(10)
            .HasDefaultValue(ScoreStatus.Fail)
            .IsRequired();

        builder.Property(e => e.ImportedDate)
            .HasColumnType("timestamp")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasIndex(e => e.RollNumber);

        builder.HasIndex(e => e.SubjectClassId);

        builder.HasOne(e => e.SubjectClass)
            .WithMany(s => s.ScoreRecords)
            .HasForeignKey(e => e.SubjectClassId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
