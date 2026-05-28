using System.Collections.Generic;

namespace DataAccessLayer.Entity;

public class SubjectClass
{
    public int Id { get; set; }

    public string SubjectCode { get; set; } = null!;
    public string SubjectName { get; set; } = null!;
    public string ClassName { get; set; } = null!;
    public string Semester { get; set; } = null!;

    public ICollection<ScoreRecord> ScoreRecords { get; set; } = new List<ScoreRecord>();
}
