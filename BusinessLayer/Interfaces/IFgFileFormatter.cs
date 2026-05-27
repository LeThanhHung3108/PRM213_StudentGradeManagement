using BusinessLayer.DTOs;
using DataAccessLayer.Entity;

namespace BusinessLayer.Interfaces;

/// <summary>
/// Converts a list of grade records into the bytes of an FG export file.
/// Implement this interface to support new file formats without touching the service layer.
/// </summary>
public interface IFgFileFormatter
{
    FgFileFormat FileFormat { get; }

    /// <summary>File extension including the leading dot, e.g. ".csv".</summary>
    string FileExtension { get; }

    string ContentType { get; }

    byte[] FormatRecords(IReadOnlyList<ImportedGradeRecord> records);
}
