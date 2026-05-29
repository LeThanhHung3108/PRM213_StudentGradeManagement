using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccessLayer.Context;
using DataAccessLayer.Entity;
using DataAccessLayer.IRepository;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repository;

public class ImportRepository : IImportRepository
{
    private readonly AppDbContext _context;

    public ImportRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<SubjectClass?> GetSubjectClassByIdAsync(int id)
    {
        return await _context.SubjectClasses.FindAsync(id);
    }

    public async Task AddScoreRecordsAsync(IEnumerable<ScoreRecord> records)
    {
        await _context.ScoreRecords.AddRangeAsync(records);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return await _context.Database.BeginTransactionAsync();
    }
}
