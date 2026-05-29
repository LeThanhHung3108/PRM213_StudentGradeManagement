using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.DTOs;

public class CreateSubjectClassDto
{
    [Required]
    [MaxLength(20)]
    public string SubjectCode { get; set; } = null!;

    [Required]
    [MaxLength(255)]
    public string SubjectName { get; set; } = null!;

    [Required]
    [MaxLength(100)]
    public string ClassName { get; set; } = null!;

    [Required]
    [MaxLength(50)]
    public string Semester { get; set; } = null!;
}
