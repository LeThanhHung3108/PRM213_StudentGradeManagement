namespace BusinessLayer.DTOs;

public class ClassStatisticsDto
{
    public string ClassName { get; set; } = string.Empty;
    public int TotalStudents { get; set; }
    public int TotalRecords { get; set; }
    public double PassRate { get; set; }
    public double AverageScore { get; set; }
}

public class SubjectStatisticsDto
{
    public string SubjectName { get; set; } = string.Empty;
    public int TotalStudents { get; set; }
    public int TotalRecords { get; set; }
    public double PassRate { get; set; }
    public double AverageScore { get; set; }
}

public class PassFailStatisticsDto
{
    public string? ClassName { get; set; }
    public string? SubjectName { get; set; }
    public int TotalRecords { get; set; }
    public int PassCount { get; set; }
    public int FailCount { get; set; }
    public double PassRate { get; set; }
    public double FailRate { get; set; }
}