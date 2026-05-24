using DataAccessLayer.Context;
using DataAccessLayer.Entity;
using DataAccessLayer.Enums;
using DataAccessLayer.IRepository;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repository;

public class ImportedGradeRecordRepository : GenericRepository<ImportedGradeRecord>, IImportedGradeRecordRepository
{
    public ImportedGradeRecordRepository(ApplicationDbContext context)
        : base(context)
    {
    }

    public Task<int> CountPassAsync() =>
        CountAsync(x => x.Result == GradeResult.Pass);

    public Task<int> CountFailAsync() =>
        CountAsync(x => x.Result == GradeResult.Fail);

    public async Task<IReadOnlyList<ImportedGradeRecord>> GetByImportHistoryIdAsync(int importHistoryId) =>
        await _dbSet.AsNoTracking()
            .Where(x => x.ImportHistoryId == importHistoryId)
            .ToListAsync();
}
