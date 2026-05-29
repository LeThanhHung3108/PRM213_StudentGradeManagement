using System.Collections.Generic;

namespace BusinessLayer.DTOs;

public class ImportResultDto
{
    public int TotalRows { get; set; }
    public int SuccessRows { get; set; }
    public int FailedRows { get; set; }
    public List<string> ErrorMessages { get; set; } = new();
}
