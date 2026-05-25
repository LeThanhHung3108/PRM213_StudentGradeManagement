namespace BusinessLayer.DTOs;

/// <summary>Aggregated data consumed by the front-end dashboard.</summary>
public class DashboardDto
{
    public int TotalStudents { get; set; }
    public int TotalImports { get; set; }
    public double OverallPassRate { get; set; }
    public IReadOnlyList<ImportHistorySummaryDto> RecentImports { get; set; } = [];
    public IReadOnlyList<GradeDistributionItemDto> GradeDistribution { get; set; } = [];
    public IReadOnlyList<SubjectAverageDto> TopSubjectsByAverage { get; set; } = [];
}

public class ImportHistorySummaryDto
{
    public int Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string UploadedBy { get; set; } = string.Empty;
    public DateTime ImportedAt { get; set; }
    public int RecordCount { get; set; }
}
