using BusinessLayer.DTOs;
using BusinessLayer.Interfaces;
using DataAccessLayer.IRepository;

namespace BusinessLayer.Services;

public class DashboardService : IDashboardService
{
    private readonly IUnitOfWork _uow;

    public DashboardService(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<DashboardDto> GetDashboardAsync(CancellationToken ct = default)
    {
        var allImports = await _uow.ImportHistories.GetAllAsync();
        var recentImports = allImports
            .OrderByDescending(x => x.ImportedAt)
            .Take(10)
            .ToList();

        var records = await _uow.GradeRecords.GetAllAsync();
        var totalStudents = records
            .Select(x => x.StudentCode)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Count();
        var totalRecords = records.Count;
        var passCount = records.Count(x => string.Equals(x.Result, DataAccessLayer.Enums.GradeResult.Pass, StringComparison.OrdinalIgnoreCase));
        var passRate = totalRecords == 0 ? 0 : Math.Round((double)passCount / totalRecords * 100, 2);

        var distribution = records
            .GroupBy(x => x.Result ?? "Unknown")
            .ToDictionary(g => g.Key, g => g.Count());

        var subjectAvg = records
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
            .OrderByDescending(x => x.AverageScore)
            .Take(5)
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

        var topSubjects = subjectAvg
            .OrderByDescending(x => x.AverageScore)
            .Take(5)
            .Select(a => new SubjectAverageDto
            {
                SubjectCode  = a.SubjectCode,
                SubjectName  = a.SubjectName,
                AverageScore = Math.Round(a.AverageScore, 2)
            })
            .ToList();

        var importDtos = recentImports
            .Select(h => new ImportHistorySummaryDto
            {
                Id          = h.Id,
                FileName    = h.FileName,
                UploadedBy  = string.Empty,
                ImportedAt  = h.ImportedAt,
                RecordCount = h.ImportedRows
            })
            .ToList();

        return new DashboardDto
        {
            TotalStudents     = totalStudents,
            TotalImports      = allImports.Count,
            OverallPassRate   = passRate,
            RecentImports     = importDtos,
            GradeDistribution = distributionDtos,
            TopSubjectsByAverage = topSubjects
        };
    }
}
