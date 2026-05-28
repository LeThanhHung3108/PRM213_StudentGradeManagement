using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Configurations;
using DataAccessLayer.Entity;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<SubjectClass> SubjectClasses { get; set; } = null!;
    public DbSet<ScoreRecord> ScoreRecords { get; set; } = null!;
    public DbSet<ScoreComponent> ScoreComponents { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new SubjectClassConfiguration());
        modelBuilder.ApplyConfiguration(new ScoreRecordConfiguration());
        modelBuilder.ApplyConfiguration(new ScoreComponentConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}
