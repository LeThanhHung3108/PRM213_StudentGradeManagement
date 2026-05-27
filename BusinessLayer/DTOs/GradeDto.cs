namespace BusinessLayer.DTOs;

public class GradeDto
{
    public int Id { get; set; }
    public string StudentCode { get; set; } = string.Empty;
    public string StudentName { get; set; } = string.Empty;
    public string ClassName { get; set; } = string.Empty;
    public string SubjectName { get; set; } = string.Empty;
    public decimal? ProcessScore { get; set; }
    public decimal? MidtermScore { get; set; }
    public decimal? FinalScore { get; set; }
    public decimal? TotalScore { get; set; }
    public string? Result { get; set; }
    public DateTime ImportedAt { get; set; }
    public int? ImportHistoryId { get; set; }
}