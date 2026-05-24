namespace DataAccessLayer.IRepository;

public interface IUnitOfWork : IDisposable
{
    IImportedGradeRecordRepository ImportedGradeRecords { get; }
    IImportHistoryRepository ImportHistories { get; }
    Task<int> SaveChangesAsync();
}
