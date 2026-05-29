using System.ComponentModel.DataAnnotations;

namespace BusinessLayer.DTOs;

public class UpdateScoreDto
{
    [Required]
    public double FinalGrade { get; set; }

    [Required]
    [MaxLength(10)]
    public string Status { get; set; } = null!;
}