using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessLayer.Context;
using DataAccessLayer.Entity;
using DataAccessLayer.IRepository;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repository;

public class StatisticsRepository : IStatisticsRepository
{
    private readonly AppDbContext _context;

    public StatisticsRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<(int TotalStudents, int PassCount, int FailCount, double Average, double Highest, double Lowest)> GetStatisticsAsync(int? subjectClassId)
    {
        var query = _context.ScoreRecords.AsNoTracking().AsQueryable();
        if (subjectClassId.HasValue) query = query.Where(x => x.SubjectClassId == subjectClassId.Value);
        var aggregate = await query
            .GroupBy(x => 1)
            .Select(g => new
            {
                Total = g.Count(),
                Pass = g.Count(x => x.FinalGrade >= 5.0),
                Average = g.Average(x => (double?)x.FinalGrade),
                Highest = g.Max(x => (double?)x.FinalGrade),
                Lowest = g.Min(x => (double?)x.FinalGrade)
            })
            .FirstOrDefaultAsync();
        if (aggregate == null)
        {
            return (0, 0, 0, 0, 0, 0);
        }
        var total = aggregate.Total;
        var pass = aggregate.Pass;
        var fail = total - pass;
        var avg = aggregate.Average ?? 0;
        var hi = aggregate.Highest ?? 0;
        var lo = aggregate.Lowest ?? 0;
        return (total, pass, fail, Math.Round(avg, 2), hi, lo);
    }

    public async Task<(List<ScoreRecord> Items, int TotalCount)> GetTopStudentsAsync(int pageNumber, int pageSize, int? subjectClassId)
    {
        if (pageNumber <= 0) pageNumber = 1;
        if (pageSize <= 0) pageSize = 10;
        var query = _context.ScoreRecords.AsNoTracking().AsQueryable();
        if (subjectClassId.HasValue) query = query.Where(x => x.SubjectClassId == subjectClassId.Value);
        var total = await query.CountAsync();
        var items = await query
            .OrderByDescending(x => x.FinalGrade)
            .ThenBy(x => x.RollNumber)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        return (items, total);
    }

    public async Task<(List<ScoreRecord> Items, int TotalCount)> GetFailedStudentsAsync(int pageNumber, int pageSize, int? subjectClassId)
    {
        if (pageNumber <= 0) pageNumber = 1;
        if (pageSize <= 0) pageSize = 10;
        var query = _context.ScoreRecords.AsNoTracking().Where(x => x.FinalGrade < 5.0);
        if (subjectClassId.HasValue) query = query.Where(x => x.SubjectClassId == subjectClassId.Value);
        var total = await query.CountAsync();
        var items = await query
            .OrderBy(x => x.FinalGrade)
            .ThenBy(x => x.RollNumber)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        return (items, total);
    }
}
