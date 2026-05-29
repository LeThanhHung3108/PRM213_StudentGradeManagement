    using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccessLayer.Entity;

namespace DataAccessLayer.IRepository;

public interface IStatisticsRepository
{
    Task<(int TotalStudents, int PassCount, int FailCount, double Average, double Highest, double Lowest)> GetStatisticsAsync(int? subjectClassId);
    Task<(List<ScoreRecord> Items, int TotalCount)> GetTopStudentsAsync(int pageNumber, int pageSize, int? subjectClassId);
    Task<(List<ScoreRecord> Items, int TotalCount)> GetFailedStudentsAsync(int pageNumber, int pageSize, int? subjectClassId);
}
