namespace DataAccessLayer.Entity;

public class ImportHistory
{
    public int Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public int ImportedRows { get; set; }
    public int FailedRows { get; set; }
    public DateTime ImportedAt { get; set; }

    public ICollection<ImportedGradeRecord> GradeRecords { get; set; } = new List<ImportedGradeRecord>();
}
