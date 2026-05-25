using DataAccessLayer.Context;
using DataAccessLayer.IRepository;

namespace DataAccessLayer.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IImportedGradeRecordRepository? _importedGradeRecords;
    private IImportHistoryRepository? _importHistories;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public IImportedGradeRecordRepository GradeRecords =>
        _importedGradeRecords ??= new ImportedGradeRecordRepository(_context);

    public IImportedGradeRecordRepository ImportedGradeRecords =>
        _importedGradeRecords ??= new ImportedGradeRecordRepository(_context);

    public IImportHistoryRepository ImportHistories =>
        _importHistories ??= new ImportHistoryRepository(_context);

    public Task<int> SaveChangesAsync() =>
        _context.SaveChangesAsync();

    public void Dispose() =>
        _context.Dispose();
}
