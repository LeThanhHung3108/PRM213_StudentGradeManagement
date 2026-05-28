using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccessLayer.Configurations
{
    public class ScoreComponentConfiguration : IEntityTypeConfiguration<ScoreComponent>
    {
        public void Configure(EntityTypeBuilder<ScoreComponent> builder)
        {
            builder.ToTable("score_components");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.ScoreRecordId)
                .IsRequired();

            builder.Property(e => e.ComponentName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.ScoreValue)
                .HasColumnType("double precision")
                .HasDefaultValue(0);

            builder.Property(e => e.Weight)
                .HasColumnType("double precision")
                .HasDefaultValue(1);

            builder.HasIndex(e => e.ScoreRecordId);

            builder.HasOne(e => e.ScoreRecord)
                .WithMany(r => r.ScoreComponents)
                .HasForeignKey(e => e.ScoreRecordId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("score_components", t =>
            {
                t.HasCheckConstraint(
                    "CK_score_components_score_value_nonneg",
                    "\"ScoreValue\" >= 0");

                t.HasCheckConstraint(
                    "CK_score_components_weight_positive",
                    "\"Weight\" > 0");
            });
        }
    }
}
