namespace BusinessLayer.DTOs;

public class StatisticsDto
{
    public int TotalStudents { get; set; }
    public int PassCount { get; set; }
    public int FailCount { get; set; }
    public double AverageScore { get; set; }
    public double HighestScore { get; set; }
    public double LowestScore { get; set; }
}