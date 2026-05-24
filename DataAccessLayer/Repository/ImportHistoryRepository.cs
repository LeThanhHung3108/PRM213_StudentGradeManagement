using DataAccessLayer.Context;
using DataAccessLayer.Entity;
using DataAccessLayer.IRepository;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repository;

public class ImportHistoryRepository : GenericRepository<ImportHistory>, IImportHistoryRepository
{
    public ImportHistoryRepository(ApplicationDbContext context)
        : base(context)
    {
    }

    public async Task<ImportHistory?> GetLatestAsync() =>
        await _dbSet.AsNoTracking()
            .OrderByDescending(x => x.ImportedAt)
            .FirstOrDefaultAsync();
}
