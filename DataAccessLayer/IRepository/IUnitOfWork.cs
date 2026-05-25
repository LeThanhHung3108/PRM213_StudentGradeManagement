namespace DataAccessLayer.IRepository;

public interface IUnitOfWork : IDisposable
{
    IImportedGradeRecordRepository GradeRecords { get; }
    IImportedGradeRecordRepository ImportedGradeRecords { get; }
    IImportHistoryRepository ImportHistories { get; }
    Task<int> SaveChangesAsync();
}
