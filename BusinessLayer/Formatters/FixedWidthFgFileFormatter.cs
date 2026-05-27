using System.Text;
using BusinessLayer.DTOs;
using BusinessLayer.Interfaces;
using DataAccessLayer.Entity;

namespace BusinessLayer.Formatters;

/// <summary>
/// Formats grade records as a fixed-width flat file suitable for legacy system ingestion.
/// Column layout:
///   StudentCode  [10]  StudentName [30]  SubjectCode [10]
///   SubjectName [30]   Score [6]  GradeResult [16]  Semester [10]
/// </summary>
public class FixedWidthFgFileFormatter : IFgFileFormatter
{
    public FgFileFormat FileFormat => FgFileFormat.FixedWidth;
    public string FileExtension => ".txt";
    public string ContentType => "text/plain";

    private const int ColStudentCode  = 10;
    private const int ColStudentName  = 30;
    private const int ColSubjectCode  = 10;
    private const int ColSubjectName  = 30;
    private const int ColScore        = 8;
    private const int ColGradeResult  = 16;
    private const int ColSemester     = 10;

    public byte[] FormatRecords(IReadOnlyList<ImportedGradeRecord> records)
    {
        var sb = new StringBuilder();

        // Header row
        sb.AppendLine(
            Pad("StudentCode",  ColStudentCode) +
            Pad("StudentName",  ColStudentName) +
            Pad("SubjectCode",  ColSubjectCode) +
            Pad("SubjectName",  ColSubjectName) +
            Pad("Score",        ColScore) +
            Pad("GradeResult",  ColGradeResult) +
            Pad("Semester",     ColSemester));

        // Separator
        sb.AppendLine(new string('-', ColStudentCode + ColStudentName + ColSubjectCode +
                                       ColSubjectName + ColScore + ColGradeResult + ColSemester));

        foreach (var r in records)
        {
            sb.AppendLine(
                Pad(r.StudentCode,             ColStudentCode) +
                Pad(r.StudentName,             ColStudentName) +
                Pad(r.ClassName,               ColSubjectCode) +
                Pad(r.SubjectName,             ColSubjectName) +
                Pad((r.TotalScore ?? 0m).ToString("F2"), ColScore) +
                Pad(r.Result ?? string.Empty,  ColGradeResult) +
                Pad(r.ImportedAt.ToString("yyyy-MM"), ColSemester));
        }

        return Encoding.UTF8.GetBytes(sb.ToString());
    }

    private static string Pad(string value, int width)
        => value.Length >= width
            ? value[..width]
            : value.PadRight(width);
}
