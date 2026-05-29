namespace BusinessLayer.DTOs;

public class TopStudentDto
{
    public int StudentId { get; set; }
    public string RollNumber { get; set; } = null!;
    public string? FullName { get; set; }
    public double FinalGrade { get; set; }
}