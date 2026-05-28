using DataAccessLayer.Enums;
using System;
using System.Collections.Generic;

namespace DataAccessLayer.Entity;

public class ScoreRecord
{
    public int Id { get; set; }

    public int SubjectClassId { get; set; }

    public string RollNumber { get; set; } = null!;

    public string? FullName { get; set; }

    public double FinalGrade { get; set; }

    public ScoreStatus Status { get; set; } = ScoreStatus.Fail;

    public DateTime ImportedDate { get; set; }

    public SubjectClass? SubjectClass { get; set; }

    public ICollection<ScoreComponent> ScoreComponents { get; set; } = new List<ScoreComponent>();
}
