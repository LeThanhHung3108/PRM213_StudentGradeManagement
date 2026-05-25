namespace BusinessLayer.DTOs;

/// <summary>Overall statistics summary for a given semester (or all time if semester is null).</summary>
public class StatisticsDto
{
    public int TotalStudents { get; set; }
    public int TotalRecords { get; set; }
    public double PassRate { get; set; }
    public string? ClassName { get; set; }
    public string? SubjectName { get; set; }
    public IReadOnlyList<GradeDistributionItemDto> GradeDistribution { get; set; } = [];
    public IReadOnlyList<SubjectAverageDto> SubjectAverages { get; set; } = [];
}

public class GradeDistributionItemDto
{
    public string GradeResult { get; set; } = string.Empty;
    public int Count { get; set; }
    public double Percentage { get; set; }
}

public class SubjectAverageDto
{
    public string SubjectCode { get; set; } = string.Empty;
    public string SubjectName { get; set; } = string.Empty;
    public double AverageScore { get; set; }
}
