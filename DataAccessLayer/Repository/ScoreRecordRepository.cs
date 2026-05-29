using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessLayer.Context;
using DataAccessLayer.Entity;
using DataAccessLayer.Enums;
using DataAccessLayer.IRepository;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repository;

public class ScoreRecordRepository : IScoreRecordRepository
{
    private readonly AppDbContext _context;

    public ScoreRecordRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<(List<ScoreRecord> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize, int? subjectClassId, string? search, string? status)
    {
        if (pageNumber <= 0) pageNumber = 1;
        if (pageSize <= 0) pageSize = 10;
        var query = _context.ScoreRecords.AsQueryable();
        if (subjectClassId.HasValue) query = query.Where(x => x.SubjectClassId == subjectClassId.Value);
        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.Trim();
            query = query.Where(x => EF.Functions.ILike(x.RollNumber, $"%{s}%") || EF.Functions.ILike(x.FullName ?? string.Empty, $"%{s}%"));
        }
        if (!string.IsNullOrWhiteSpace(status) && Enum.TryParse<ScoreStatus>(status, true, out var parsed))
        {
            query = query.Where(x => x.Status == parsed);
        }
        var totalCount = await query.CountAsync();
        var items = await query
            .AsNoTracking()
            .OrderByDescending(x => x.ImportedDate)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        return (items, totalCount);
    }

    public async Task<ScoreRecord?> GetByIdAsync(int id)
    {
        return await _context.ScoreRecords
            .Include(x => x.ScoreComponents)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public void Update(ScoreRecord entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));
        _context.ScoreRecords.Update(entity);
    }

    public void Delete(ScoreRecord entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));
        _context.ScoreRecords.Remove(entity);
    }

    public async Task AddAsync(ScoreRecord entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));
        await _context.ScoreRecords.AddAsync(entity);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}