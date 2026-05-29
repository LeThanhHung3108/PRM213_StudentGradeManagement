using System;

namespace BusinessLayer.DTOs;

public class SubjectClassDto
{
    public int Id { get; set; }
    public string SubjectCode { get; set; } = null!;
    public string SubjectName { get; set; } = null!;
    public string ClassName { get; set; } = null!;
    public string Semester { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}
