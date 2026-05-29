using System.Collections.Generic;

namespace BusinessLayer.DTOs;

public class ScoreDetailDto
{
    public int Id { get; set; }
    public string RollNumber { get; set; } = null!;
    public string? FullName { get; set; }
    public double FinalGrade { get; set; }
    public string Status { get; set; } = null!;
    public IEnumerable<ScoreComponentDto> Components { get; set; } = new List<ScoreComponentDto>();
}

public class ScoreComponentDto
{
    public string ComponentName { get; set; } = null!;
    public double ScoreValue { get; set; }
}