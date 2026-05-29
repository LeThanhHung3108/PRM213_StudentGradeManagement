using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccessLayer.Entity;

namespace DataAccessLayer.IRepository;

public interface IScoreRecordRepository
{
    Task<(List<ScoreRecord> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize, int? subjectClassId, string? search, string? status);
    Task<ScoreRecord?> GetByIdAsync(int id);
    void Update(ScoreRecord entity);
    void Delete(ScoreRecord entity);
    Task AddAsync(ScoreRecord entity);
    Task<int> SaveChangesAsync();
}