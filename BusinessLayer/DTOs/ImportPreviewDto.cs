using System.Collections.Generic;

namespace BusinessLayer.DTOs;

public class ImportPreviewDto
{
    public IEnumerable<string> Headers { get; set; } = new List<string>();
    public int TotalRows { get; set; }
    public IEnumerable<IDictionary<string, string?>> SampleRows { get; set; } = new List<IDictionary<string, string?>>();
}
