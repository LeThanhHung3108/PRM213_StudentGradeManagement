using DataAccessLayer.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Configurations;

public class ImportHistoryConfiguration : IEntityTypeConfiguration<ImportHistory>
{
    public void Configure(EntityTypeBuilder<ImportHistory> builder)
    {
        builder.ToTable("ImportHistories");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.FileName)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.ImportedRows)
            .IsRequired();

        builder.Property(x => x.FailedRows)
            .IsRequired();

        builder.Property(x => x.ImportedAt)
            .IsRequired();

        builder.HasIndex(x => x.ImportedAt);
    }
}
