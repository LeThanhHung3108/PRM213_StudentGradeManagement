using DataAccessLayer.Entity;

namespace DataAccessLayer.IRepository;

public interface IImportHistoryRepository : IGenericRepository<ImportHistory>
{
    Task<ImportHistory?> GetLatestAsync();
}
