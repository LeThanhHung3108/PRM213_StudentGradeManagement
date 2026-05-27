using BusinessLayer.DTOs;
using BusinessLayer.Interfaces;
using DataAccessLayer.IRepository;

namespace BusinessLayer.Services;

public class StatisticsService : IStatisticsService
{
    private readonly IUnitOfWork _uow;

    public StatisticsService(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<StatisticsDto> GetOverviewAsync(string? className = null, string? subjectName = null, CancellationToken ct = default)
    {
        var records = await _uow.GradeRecords.GetAllAsync();
        records = FilterRecords(records, className, subjectName);

        var totalStudents = records
            .Select(x => x.StudentCode)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Count();

        var totalRecords = records.Count;

        var distribution = records
            .GroupBy(x => x.Result ?? "Unknown")
            .ToDictionary(g => g.Key, g => g.Count());

        var passCount = records.Count(x => string.Equals(x.Result, DataAccessLayer.Enums.GradeResult.Pass, StringComparison.OrdinalIgnoreCase));
        var passRate = totalRecords == 0 ? 0 : Math.Round((double)passCount / totalRecords * 100, 2);

        var averages = records
            .GroupBy(x => new { x.ClassName, x.SubjectName })
            .Select(g => new
            {
                SubjectCode = g.Key.ClassName,
                SubjectName = g.Key.SubjectName,
                AverageScore = g.Where(x => x.TotalScore.HasValue)
                    .Select(x => (double)x.TotalScore!.Value)
                    .DefaultIfEmpty(0)
                    .Average()
            })
            .ToList();

        var distributionDtos = distribution
            .Select(kvp => new GradeDistributionItemDto
            {
                GradeResult = kvp.Key,
                Count = kvp.Value,
                Percentage = totalRecords == 0 ? 0 : Math.Round((double)kvp.Value / totalRecords * 100, 2)
            })
            .OrderBy(x => x.GradeResult)
            .ToList();

        var averageDtos = averages
            .Select(a => new SubjectAverageDto
            {
                SubjectCode  = a.SubjectCode,
                SubjectName  = a.SubjectName,
                AverageScore = Math.Round(a.AverageScore, 2)
            })
            .OrderBy(x => x.SubjectCode)
            .ToList();

        return new StatisticsDto
        {
            TotalStudents     = totalStudents,
            TotalRecords      = totalRecords,
            PassRate          = passRate,
            ClassName         = className,
            SubjectName       = subjectName,
            GradeDistribution = distributionDtos,
            SubjectAverages   = averageDtos
        };
    }

    public async Task<IReadOnlyList<ClassStatisticsDto>> GetByClassAsync(string? className = null, string? subjectName = null, CancellationToken ct = default)
    {
        var records = FilterRecords(await _uow.GradeRecords.GetAllAsync(), className, subjectName);

        return records
            .GroupBy(x => x.ClassName)
            .Select(group => new ClassStatisticsDto
            {
                ClassName = group.Key,
                TotalStudents = group.Select(x => x.StudentCode).Distinct(StringComparer.OrdinalIgnoreCase).Count(),
                TotalRecords = group.Count(),
                PassRate = group.Count(x => string.Equals(x.Result, DataAccessLayer.Enums.GradeResult.Pass, StringComparison.OrdinalIgnoreCase)) * 100d / group.Count(),
                AverageScore = Math.Round(group.Where(x => x.TotalScore.HasValue).Select(x => (double)x.TotalScore!.Value).DefaultIfEmpty(0).Average(), 2)
            })
            .OrderBy(x => x.ClassName)
            .ToList();
    }

    public async Task<IReadOnlyList<SubjectStatisticsDto>> GetBySubjectAsync(string? className = null, string? subjectName = null, CancellationToken ct = default)
    {
        var records = FilterRecords(await _uow.GradeRecords.GetAllAsync(), className, subjectName);

        return records
            .GroupBy(x => x.SubjectName)
            .Select(group => new SubjectStatisticsDto
            {
                SubjectName = group.Key,
                TotalStudents = group.Select(x => x.StudentCode).Distinct(StringComparer.OrdinalIgnoreCase).Count(),
                TotalRecords = group.Count(),
                PassRate = group.Count(x => string.Equals(x.Result, DataAccessLayer.Enums.GradeResult.Pass, StringComparison.OrdinalIgnoreCase)) * 100d / group.Count(),
                AverageScore = Math.Round(group.Where(x => x.TotalScore.HasValue).Select(x => (double)x.TotalScore!.Value).DefaultIfEmpty(0).Average(), 2)
            })
            .OrderBy(x => x.SubjectName)
            .ToList();
    }

    public async Task<PassFailStatisticsDto> GetPassFailAsync(string? className = null, string? subjectName = null, CancellationToken ct = default)
    {
        var records = FilterRecords(await _uow.GradeRecords.GetAllAsync(), className, subjectName);
        var totalRecords = records.Count;
        var passCount = records.Count(x => string.Equals(x.Result, DataAccessLayer.Enums.GradeResult.Pass, StringComparison.OrdinalIgnoreCase));
        var failCount = records.Count(x => string.Equals(x.Result, DataAccessLayer.Enums.GradeResult.Fail, StringComparison.OrdinalIgnoreCase));

        return new PassFailStatisticsDto
        {
            ClassName = className,
            SubjectName = subjectName,
            TotalRecords = totalRecords,
            PassCount = passCount,
            FailCount = failCount,
            PassRate = totalRecords == 0 ? 0 : Math.Round((double)passCount / totalRecords * 100, 2),
            FailRate = totalRecords == 0 ? 0 : Math.Round((double)failCount / totalRecords * 100, 2)
        };
    }

    private static IReadOnlyList<DataAccessLayer.Entity.ImportedGradeRecord> FilterRecords(
        IReadOnlyList<DataAccessLayer.Entity.ImportedGradeRecord> records,
        string? className,
        string? subjectName)
    {
        var query = records.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(className))
        {
            query = query.Where(x => string.Equals(x.ClassName, className, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(subjectName))
        {
            query = query.Where(x => string.Equals(x.SubjectName, subjectName, StringComparison.OrdinalIgnoreCase));
        }

        return query.ToList();
    }
}
