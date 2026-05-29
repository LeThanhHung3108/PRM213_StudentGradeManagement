using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccessLayer.Entity;
using Microsoft.EntityFrameworkCore.Storage;

namespace DataAccessLayer.IRepository;

public interface IImportRepository
{
    Task<SubjectClass?> GetSubjectClassByIdAsync(int id);
    Task AddScoreRecordsAsync(IEnumerable<ScoreRecord> records);
    Task<int> SaveChangesAsync();
    Task<IDbContextTransaction> BeginTransactionAsync();
}
