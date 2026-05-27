namespace BusinessLayer.DTOs;

/// <summary>Parameters for generating an FG (Final Grade) export file.</summary>
public class FgGenerateRequestDto
{
    /// <summary>Target semester, e.g. "2024-1". Null = all semesters.</summary>
    public string? Semester { get; set; }

    /// <summary>Desired file format. Defaults to CSV.</summary>
    public FgFileFormat Format { get; set; } = FgFileFormat.Csv;
}

public enum FgFileFormat
{
    Csv,
    Tsv,
    FixedWidth
}

/// <summary>Result returned after a successful FG generation.</summary>
public class FgGenerateResultDto
{
    /// <summary>Opaque token used to download the file via GET /fg/download/{token}.</summary>
    public string DownloadToken { get; set; } = string.Empty;

    public string FileName { get; set; } = string.Empty;

    /// <summary>MIME content type for the generated file.</summary>
    public string ContentType { get; set; } = string.Empty;

    public int RecordCount { get; set; }

    public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
}

/// <summary>In-memory representation of a generated FG file ready for streaming.</summary>
public class FgFileDto
{
    public string FileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public byte[] Content { get; set; } = [];
}
