using System.Text;
using BusinessLayer.DTOs;
using BusinessLayer.Interfaces;
using DataAccessLayer.Entity;

namespace BusinessLayer.Formatters;

/// <summary>Formats grade records as a tab-separated values file.</summary>
public class TsvFgFileFormatter : IFgFileFormatter
{
    public FgFileFormat FileFormat => FgFileFormat.Tsv;
    public string FileExtension => ".tsv";
    public string ContentType => "text/tab-separated-values";

    public byte[] FormatRecords(IReadOnlyList<ImportedGradeRecord> records)
    {
        var sb = new StringBuilder();
        sb.AppendLine("StudentCode\tStudentName\tSubjectCode\tSubjectName\tScore\tGradeResult\tSemester");

        foreach (var r in records)
        {
            sb.AppendLine(string.Join('\t',
                r.StudentCode,
                r.StudentName,
                r.ClassName,
                r.SubjectName,
                (r.TotalScore ?? 0m).ToString("F2"),
                r.Result ?? string.Empty,
                r.ImportedAt.ToString("yyyy-MM")));
        }

        return Encoding.UTF8.GetBytes(sb.ToString());
    }
}
