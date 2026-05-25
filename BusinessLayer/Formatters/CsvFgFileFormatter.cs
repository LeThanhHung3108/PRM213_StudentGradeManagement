using System.Text;
using BusinessLayer.DTOs;
using BusinessLayer.Interfaces;
using DataAccessLayer.Entity;

namespace BusinessLayer.Formatters;

/// <summary>Formats grade records as a UTF-8 CSV file.</summary>
public class CsvFgFileFormatter : IFgFileFormatter
{
    public FgFileFormat FileFormat => FgFileFormat.Csv;
    public string FileExtension => ".csv";
    public string ContentType => "text/csv";

    public byte[] FormatRecords(IReadOnlyList<ImportedGradeRecord> records)
    {
        var sb = new StringBuilder();

        // Header
        sb.AppendLine("StudentCode,StudentName,SubjectCode,SubjectName,Score,GradeResult,Semester");

        foreach (var r in records)
        {
            sb.AppendLine(string.Join(',',
                Escape(r.StudentCode),
                Escape(r.StudentName),
                Escape(r.ClassName),
                Escape(r.SubjectName),
                (r.TotalScore ?? 0m).ToString("F2"),
                Escape(r.Result ?? string.Empty),
                Escape(r.ImportedAt.ToString("yyyy-MM"))));
        }

        return Encoding.UTF8.GetBytes(sb.ToString());
    }

    private static string Escape(string value)
    {
        if (value.Contains(',') || value.Contains('"') || value.Contains('\n'))
            return $"\"{value.Replace("\"", "\"\"")}\"";
        return value;
    }
}
