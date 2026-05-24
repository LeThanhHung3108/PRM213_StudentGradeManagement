using DataAccessLayer.Entity;

namespace DataAccessLayer.IRepository;

public interface IImportedGradeRecordRepository : IGenericRepository<ImportedGradeRecord>
{
    Task<int> CountPassAsync();
    Task<int> CountFailAsync();
    Task<IReadOnlyList<ImportedGradeRecord>> GetByImportHistoryIdAsync(int importHistoryId);
}
