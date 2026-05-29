namespace BusinessLayer.DTOs;

public class ScoreRecordDto
{
    public int Id { get; set; }
    public string RollNumber { get; set; } = null!;
    public string? FullName { get; set; }
    public double FinalGrade { get; set; }
    public string Status { get; set; } = null!;
}